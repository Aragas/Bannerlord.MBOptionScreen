using MCM.Abstractions;
using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Containers.Global;
using MCM.Logger;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MCM.Implementation.Settings.Containers.Global
{
    internal sealed class MCMGlobalSettingsContainer : BaseGlobalSettingsContainer, IMCMGlobalSettingsContainer
    {
        public MCMGlobalSettingsContainer(IMCMLogger<MCMGlobalSettingsContainer> logger)
        {
            var settings = new List<GlobalSettings>();
            var allTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => !a.IsDynamic)
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => t.GetConstructor(Type.EmptyTypes) is not null)
                .ToList();

            var mbOptionScreenSettings = allTypes
                .Where(t => typeof(GlobalSettings).IsAssignableFrom(t))
                .Where(t => !typeof(EmptyGlobalSettings).IsAssignableFrom(t))
                .Where(t => !typeof(IWrapper).IsAssignableFrom(t))
                .Select(t => Activator.CreateInstance(t) as GlobalSettings)
                .Where(t => t is not null)
                .Cast<GlobalSettings>();
            settings.AddRange(mbOptionScreenSettings);

            foreach (var setting in settings)
            {
                logger.LogTrace("Registering settings {type}.", setting.GetType());
                RegisterSettings(setting);
            }
        }
    }
}