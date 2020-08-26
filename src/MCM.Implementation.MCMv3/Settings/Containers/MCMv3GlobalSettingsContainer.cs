extern alias v3;
extern alias v4;

using MCM.Implementation.MCMv3.Settings.Base;

using System;
using System.Collections.Generic;
using System.Linq;

using v4::MCM.Abstractions.Settings.Base.Global;
using v4::MCM.Abstractions.Settings.Containers.Global;
using v4::MCM.Utils;

namespace MCM.Implementation.MCMv3.Settings.Containers
{
    internal sealed class MCMv3GlobalSettingsContainer : BaseGlobalSettingsContainer, IMCMv3GlobalSettingsContainer
    {
        public MCMv3GlobalSettingsContainer()
        {
            var settings = new List<GlobalSettings>();
            var allTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => !a.IsDynamic)
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => t.GetConstructor(Type.EmptyTypes) != null)
                .ToList();

            var mbOptionScreenSettings = allTypes
                .Where(t => typeof(v3::MCM.Abstractions.Settings.Base.Global.GlobalSettings).IsAssignableFrom(t))
                .Where(t => !typeof(v3::MCM.Abstractions.Settings.Base.Global.EmptyGlobalSettings).IsAssignableFrom(t))
                .Where(t => !ReflectionUtils.ImplementsOrImplementsEquivalent(t, "MCM.MCMSettings"))
                .Where(t => !typeof(v3::MCM.Abstractions.IWrapper).IsAssignableFrom(t))
                .Select(obj => new MCMv3GlobalSettingsWrapper(Activator.CreateInstance(obj)));
            settings.AddRange(mbOptionScreenSettings);

            foreach (var setting in settings)
                RegisterSettings(setting);
        }
    }
}