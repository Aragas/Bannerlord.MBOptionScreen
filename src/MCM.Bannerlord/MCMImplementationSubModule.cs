using BUTR.DependencyInjection;
using BUTR.DependencyInjection.Extensions;

using MCM.Abstractions;
using MCM.Abstractions.GameFeatures;
using MCM.Abstractions.Properties;
using MCM.Implementation;
using MCM.Implementation.FluentBuilder;
using MCM.Implementation.Global;
using MCM.Implementation.PerCampaign;
using MCM.Implementation.PerSave;
using MCM.Internal.Extensions;
using MCM.Internal.GameFeatures;

using System;
using System.IO;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;

using Path = System.IO.Path;

namespace MCM.Internal
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if BANNERLORDMCM_NOT_SOURCE
    internal
#else
    public
# endif
    class MCMImplementationSubModule : MBSubModuleBase, IGameEventListener
    {
        /// <inheritdoc />
        public event Action? GameStarted;

        /// <inheritdoc />
        public event Action? GameLoaded;

        /// <inheritdoc />
        public event Action? GameEnded;

        private bool ServiceRegistrationWasCalled { get; set; }

        public void OnServiceRegistration()
        {
            ServiceRegistrationWasCalled = true;

            if (this.GetServiceContainer() is { } services)
            {
                services.AddSettingsContainer<FluentGlobalSettingsContainer>();
                services.AddSettingsContainer<ExternalGlobalSettingsContainer>();
                services.AddSettingsContainer<GlobalSettingsContainer>();

                services.AddSettingsContainer<FluentPerSaveSettingsContainer>();
                services.AddSettingsContainer<PerSaveSettingsContainer>();

                services.AddSettingsContainer<FluentPerCampaignSettingsContainer>();
                services.AddSettingsContainer<PerCampaignSettingsContainer>();

                services.AddSettingsFormat<JsonSettingsFormat>();
                services.AddSettingsFormat<XmlSettingsFormat>();

                services.AddSettingsPropertyDiscoverer<IAttributeSettingsPropertyDiscoverer, AttributeSettingsPropertyDiscoverer>();
                services.AddSettingsPropertyDiscoverer<IFluentSettingsPropertyDiscoverer, FluentSettingsPropertyDiscoverer>();

                services.AddSettingsBuilderFactory<DefaultSettingsBuilderFactory>();

                services.AddSettingsProvider<DefaultSettingsProvider>();


                services.AddSingleton<IGameEventListener, MCMImplementationSubModule>(sp => this);
                services.AddSingleton<ICampaignIdProvider, CampaignIdProvider>();
                services.AddSingleton<IFileSystemProvider, FileSystemProvider>();
                services.AddScoped<PerSaveCampaignBehavior>();
                services.AddTransient<IPerSaveSettingsProvider, PerSaveCampaignBehavior>(sp => sp.GetService<PerSaveCampaignBehavior>());
            }
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            PerformMigration001();
            PerformMigration002();

            if (!ServiceRegistrationWasCalled)
                OnServiceRegistration();
        }

        /// <inheritdoc />
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            /*
            var logger = GenericServiceProvider.GetService<IBUTRLogger<MCMImplementationSubModule>>() ?? new DefaultBUTRLogger<MCMImplementationSubModule>();
            if (ModuleInfoHelper.GetModuleByType(typeof(MCMImplementationSubModule)) is { } module)
            {
                foreach (var file in Directory.GetFiles(module.Path, "*.xml", SearchOption.TopDirectoryOnly))
                {
                    var externalGlobalSettings = ExternalGlobalSettings.CreateFromXmlFile(file);
                    if (externalGlobalSettings is null) continue;

                    logger.LogTrace($"Registering settings {externalGlobalSettings.GetType()}.");
                    externalGlobalSettings.Register();
                }
            }
            */

        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            if (game.GameType is Campaign)
            {
                GameStarted?.Invoke();

                var gameStarter = (CampaignGameStarter) gameStarterObject;
                gameStarter.AddBehavior(GenericServiceProvider.GetService<PerSaveCampaignBehavior>());
            }
        }

        /// <inheritdoc />
        public override void OnNewGameCreated(Game game, object initializerObject)
        {
            base.OnNewGameCreated(game, initializerObject);

            if (game.GameType is Campaign)
            {
                GameLoaded?.Invoke();
            }
        }

        /// <inheritdoc />
        public override void OnGameLoaded(Game game, object initializerObject)
        {
            base.OnGameLoaded(game, initializerObject);

            if (game.GameType is Campaign)
            {
                GameLoaded?.Invoke();
            }
        }

        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);

            if (game.GameType is Campaign)
            {
                GameEnded?.Invoke();
            }
        }

        private static void PerformMigration001()
        {
            static void MoveDirectory(string source, string target)
            {
                var sourcePath = source.TrimEnd('\\', ' ');
                var targetPath = target.TrimEnd('\\', ' ');
                foreach (var folder in Directory.EnumerateFiles(sourcePath, "*", SearchOption.AllDirectories).GroupBy(Path.GetDirectoryName))
                {
                    var targetFolder = folder.Key.Replace(sourcePath, targetPath);
                    Directory.CreateDirectory(targetFolder);
                    foreach (var file in folder)
                    {
                        var targetFile = Path.Combine(targetFolder, Path.GetFileName(file));
                        if (File.Exists(targetFile)) File.Delete(targetFile);
                        File.Move(file, targetFile);
                    }
                }
                Directory.Delete(source, true);
            }

            try
            {
                var oldConfigPath = Path.GetFullPath("Configs");
                var oldPath = Path.Combine(oldConfigPath, "ModSettings");
                var newPath = Path.Combine(PlatformFileHelperPCExtended.GetDirectoryFullPath(EngineFilePaths.ConfigsPath) ?? string.Empty, "ModSettings");
                if (Directory.Exists(oldPath) && Directory.Exists(newPath))
                {
                    foreach (var filePath in Directory.GetFiles(oldPath))
                    {
                        var fileName = Path.GetFileName(filePath);
                        var newFilePath = Path.Combine(newPath, fileName);
                        try
                        {
                            File.Copy(filePath, newFilePath, true);
                            File.Delete(filePath);
                        }
                        catch (Exception) { }
                    }

                    foreach (var directoryPath in Directory.GetDirectories(oldPath))
                    {
                        var directoryName = Path.GetFileName(directoryPath);
                        var newDirectoryPath = Path.Combine(newPath, directoryName);
                        try
                        {
                            MoveDirectory(directoryPath, newDirectoryPath);
                        }
                        catch (Exception) { }
                    }

                    if (Directory.GetFiles(oldPath) is { Length: 0 } && Directory.GetDirectories(oldPath) is { Length: 0 })
                        Directory.Delete(oldPath, true);
                    if (Directory.GetFiles(oldConfigPath) is { Length: 0 } && Directory.GetDirectories(oldConfigPath) is { Length: 0 })
                        Directory.Delete(oldConfigPath, true);
                }
            }
            catch (Exception) { }
        }

        private static void PerformMigration002()
        {
            static void MoveDirectory(string source, string target)
            {
                var sourcePath = source.TrimEnd('\\', ' ');
                var targetPath = target.TrimEnd('\\', ' ');
                foreach (var folder in Directory.EnumerateFiles(sourcePath, "*", SearchOption.AllDirectories).GroupBy(Path.GetDirectoryName))
                {
                    var targetFolder = folder.Key.Replace(sourcePath, targetPath);
                    Directory.CreateDirectory(targetFolder);
                    foreach (var file in folder)
                    {
                        var targetFile = Path.Combine(targetFolder, Path.GetFileName(file));
                        if (File.Exists(targetFile)) File.Delete(targetFile);
                        File.Move(file, targetFile);
                    }
                }
                Directory.Delete(source, true);
            }

            try
            {
                var oldConfigPath = Path.GetFullPath("Configs");
                var oldPath = Path.GetFullPath(Path.Combine(PlatformFileHelperPCExtended.GetDirectoryFullPath(EngineFilePaths.ConfigsPath) ?? string.Empty, "../", "ModSettings"));
                var newPath = Path.Combine(PlatformFileHelperPCExtended.GetDirectoryFullPath(EngineFilePaths.ConfigsPath) ?? string.Empty, "ModSettings");
                if (Directory.Exists(oldPath) && Directory.Exists(newPath))
                {
                    foreach (var filePath in Directory.GetFiles(oldPath))
                    {
                        var fileName = Path.GetFileName(filePath);
                        var newFilePath = Path.Combine(newPath, fileName);
                        try
                        {
                            File.Copy(filePath, newFilePath, true);
                            File.Delete(filePath);
                        }
                        catch (Exception) { }
                    }

                    foreach (var directoryPath in Directory.GetDirectories(oldPath))
                    {
                        var directoryName = Path.GetFileName(directoryPath);
                        var newDirectoryPath = Path.Combine(newPath, directoryName);
                        try
                        {
                            MoveDirectory(directoryPath, newDirectoryPath);
                        }
                        catch (Exception) { }
                    }

                    if (Directory.GetFiles(oldPath) is { Length: 0 } && Directory.GetDirectories(oldPath) is { Length: 0 })
                        Directory.Delete(oldPath, true);
                    if (Directory.GetFiles(oldConfigPath) is { Length: 0 } && Directory.GetDirectories(oldConfigPath) is { Length: 0 })
                        Directory.Delete(oldConfigPath, true);
                }
            }
            catch (Exception) { }
        }
    }
}