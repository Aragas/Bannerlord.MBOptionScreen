using Bannerlord.BUTR.Shared.Helpers;

using BUTR.DependencyInjection.Logger;

using MCM.Abstractions.Base.Global;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MCM.Internal
{
    /// <summary>
    /// Registers the settings that loaded modules declare in their <c>SubModule.xml</c> (<c>&lt;Module&gt;&lt;Settings&gt;</c>).
    /// <para>
    /// Exactly one MCM instance must own a declared block, otherwise the options screen would list it twice:
    /// when the standalone module is loaded it registers every module's declarations and embedded copies of MCM
    /// stay out. Without the standalone there is no options screen, so every embedded copy registers all of them
    /// and each mod can read and save its settings through its own <c>BaseSettingsProvider</c>.
    /// </para>
    /// </summary>
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
    internal static class ModuleXmlSettingsLoader
    {
        public const string StandaloneModuleId = "Bannerlord.MBOptionScreen";

        public static void RegisterDeclaredSettings(IBUTRLogger logger)
        {
            List<ModuleInfoExtendedHelper> loadedModules;
            try
            {
                loadedModules = ModuleInfoHelper.GetLoadedModules().ToList();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to enumerate the loaded modules, settings declared in SubModule.xml are not available");
                return;
            }

            var ownModule = ModuleInfoHelper.GetModuleByType(typeof(ModuleXmlSettingsLoader));
            var isStandalone = ownModule is not null && string.Equals(ownModule.Id, StandaloneModuleId, StringComparison.OrdinalIgnoreCase);
            var standaloneLoaded = loadedModules.Any(x => string.Equals(x.Id, StandaloneModuleId, StringComparison.OrdinalIgnoreCase));

            if (!isStandalone && standaloneLoaded)
            {
                logger.LogTrace("The standalone MCM module registers the settings declared in SubModule.xml files, skipping");
                return;
            }

            // Settings id to the module that declared it first; the container keeps the first registration only
            var owners = new Dictionary<string, string>();
            foreach (var module in loadedModules)
            {
                var path = Path.Combine(module.Path, ModuleInfoHelper.SubModuleFile);
                try
                {
                    if (!File.Exists(path)) continue;

                    foreach (var settings in ExternalGlobalSettings.CreateFromFile(path))
                    {
                        if (owners.TryGetValue(settings.Id, out var owner))
                        {
                            logger.LogWarning($"Settings '{settings.Id}' declared by module '{module.Id}' are ignored, module '{owner}' already declares settings with that id");
                            continue;
                        }
                        owners.Add(settings.Id, module.Id);

                        logger.LogTrace($"Registering settings '{settings.Id}' declared by module '{module.Id}'");
                        settings.Register();
                    }
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Failed to read the settings declared in '{path}'");
                }
            }
        }
    }
}
