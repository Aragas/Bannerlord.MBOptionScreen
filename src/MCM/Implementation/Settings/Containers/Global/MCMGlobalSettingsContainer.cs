using BUTR.DependencyInjection.Logger;

using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions;
using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Containers.Global;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MCM.Implementation.Settings.Containers.Global
{
    internal sealed class MCMGlobalSettingsContainer : BaseGlobalSettingsContainer, IMCMGlobalSettingsContainer
    {
        public MCMGlobalSettingsContainer(IBUTRLogger<MCMGlobalSettingsContainer> logger)
        {
            IEnumerable<GlobalSettings> GetGlobalSettings()
            {
                foreach (var assembly in AccessTools2.AllAssemblies().Where(a => !a.IsDynamic))
                {
                    IEnumerable<GlobalSettings> settings;
                    try
                    {
                        settings = AccessTools2.GetTypesFromAssemblyIfValid(assembly)
                            .Where(t => t.IsClass && !t.IsAbstract)
                            .Where(t => t.GetConstructor(Type.EmptyTypes) is not null)
                            .Where(t => typeof(GlobalSettings).IsAssignableFrom(t))
                            .Where(t => !typeof(EmptyGlobalSettings).IsAssignableFrom(t))
                            .Where(t => !typeof(IWrapper).IsAssignableFrom(t))
                            .Select(t => Activator.CreateInstance(t) as GlobalSettings)
                            .OfType<GlobalSettings>()
                            .ToList();
                    }
                    catch (TypeLoadException ex)
                    {
                        settings = Array.Empty<GlobalSettings>();
                        logger.LogError(ex, "Error while handling assembly {Assembly}!", assembly);
                    }

                    foreach (var setting in settings)
                    {
                        yield return setting;
                    }
                }
            }

            foreach (var setting in GetGlobalSettings())
            {
                logger.LogTrace("Registering settings {type}.", setting.GetType());
                RegisterSettings(setting);
            }
        }
    }
}