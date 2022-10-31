﻿using Bannerlord.BUTR.Shared.Helpers;

using BUTR.DependencyInjection;
using BUTR.DependencyInjection.Extensions;
using BUTR.DependencyInjection.Logger;

using MCM.Abstractions;
using MCM.Abstractions.Base.Global;
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
    internal sealed class MCMImplementationSubModule : MBSubModuleBase, IGameEventListener
    {
        private static string PathPrefix => System.IO.Path.Combine(GenericServiceProvider.GetService<IPathProvider>()?.GetGamePath(), "Modules");

        /// <inheritdoc />
        public event Action? OnGameStarted;

        /// <inheritdoc />
        public event Action? OnGameEnded;

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

                services.AddSettingsBuilderFactory<DefaultSettingsBuilderFactory>();

                services.AddSettingsProvider<DefaultSettingsProvider>();


                services.AddSingleton<IGameEventListener, MCMImplementationSubModule>(sp => this);
                services.AddSingleton<ICampaignIdProvider, CampaignIdProvider>();
                services.AddSingleton<IPathProvider, PathProvider>();
                services.AddScoped<PerSaveCampaignBehavior>();
                services.AddTransient<IPerSaveSettingsProvider, PerSaveCampaignBehavior>(sp => sp.GetService<PerSaveCampaignBehavior>());
            }
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            PerformMigration001();

            if (!ServiceRegistrationWasCalled)
                OnServiceRegistration();
        }

        /// <inheritdoc />
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            var logger = GenericServiceProvider.GetService<IBUTRLogger<MCMImplementationSubModule>>() ?? new DefaultBUTRLogger<MCMImplementationSubModule>();
            foreach (var moduleDirectory in Utilities.GetModulesNames().Select(x => new DirectoryInfo(System.IO.Path.Combine(PathPrefix, x, "Settings"))))
            {
                if (!moduleDirectory.Exists) continue;

                foreach (var file in moduleDirectory.GetFiles("*.xml", SearchOption.TopDirectoryOnly))
                {
                    var externalGlobalSettings = ExternalGlobalSettings.CreateFromXmlFile(file.FullName);
                    if (externalGlobalSettings is null) continue;

                    logger.LogTrace($"Registering settings {externalGlobalSettings.GetType()}.");
                    externalGlobalSettings.Register();
                }
            }
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            if (game.GameType is Campaign)
            {
                OnGameStarted?.Invoke();

                var gameStarter = (CampaignGameStarter) gameStarterObject;
                gameStarter.AddBehavior(GenericServiceProvider.GetService<PerSaveCampaignBehavior>());
            }
        }
        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);

            if (game.GameType is Campaign)
            {
                OnGameEnded?.Invoke();
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
                    catch { }
                }
                foreach (var directoryPath in Directory.GetDirectories(oldPath))
                {
                    var directoryName = Path.GetFileName(directoryPath);
                    var newDirectoryPath = Path.Combine(newPath, directoryName);
                    try
                    {
                        MoveDirectory(directoryPath, newDirectoryPath);
                    }
                    catch { }
                }
            }
            if (Directory.GetFiles(oldPath) is { Length: 0 } && Directory.GetDirectories(oldPath) is { Length: 0})
                Directory.Delete(oldPath, true);
            if (Directory.GetFiles(oldConfigPath) is { Length: 0 } && Directory.GetDirectories(oldConfigPath) is { Length: 0})
                Directory.Delete(oldConfigPath, true);
        }
    }
}