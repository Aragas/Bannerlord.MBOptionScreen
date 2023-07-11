using BUTR.DependencyInjection.Logger;

using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions.Base.Global;
using MCM.Abstractions.Global;
using MCM.Common;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MCM.Implementation.Global
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
    internal sealed class GlobalSettingsContainer : BaseGlobalSettingsContainer, IGlobalSettingsContainer
    {
        public GlobalSettingsContainer(IBUTRLogger<GlobalSettingsContainer> logger)
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
                            .Select(t =>
                            {
                                try
                                {
                                    return Activator.CreateInstance(t) as GlobalSettings;
                                }
                                catch (Exception e)
                                {
                                    try
                                    {
                                        logger.LogError(e, $"Failed to initialize type {t}");
                                    }
                                    catch (Exception)
                                    {
                                        logger.LogError(e, "Failed to initialize and log type!");
                                    }
                                    return null;
                                }
                            })
                            .OfType<GlobalSettings>()
                            .ToList();
                    }
                    catch (TypeLoadException ex)
                    {
                        settings = Array.Empty<GlobalSettings>();
                        logger.LogError(ex, $"Error while handling assembly {assembly}!");
                    }

                    foreach (var setting in settings)
                    {
                        yield return setting;
                    }
                }
            }

            foreach (var setting in GetGlobalSettings())
            {
                logger.LogTrace($"Registering settings {setting.GetType()}.");
                RegisterSettings(setting);
            }
        }
    }
}