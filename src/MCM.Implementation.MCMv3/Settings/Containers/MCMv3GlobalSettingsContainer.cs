extern alias v3;
extern alias v4;

using Bannerlord.ButterLib;
using Bannerlord.ButterLib.Common.Extensions;

using MCM.Implementation.MCMv3.Settings.Base;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;

using v4::MCM.Abstractions.Settings.Base.Global;
using v4::MCM.Abstractions.Settings.Containers.Global;
using v4::MCM.Abstractions.Settings.Formats;
using v4::MCM.Utils;

namespace MCM.Implementation.MCMv3.Settings.Containers
{
    // MCMv3 had the ability to get Instance in OnSubModuleLoad()
    internal sealed class MCMv3GlobalSettingsContainer : BaseGlobalSettingsContainer
    {
        private static List<GlobalSettings>? Settings { get; set; }

        public MCMv3GlobalSettingsContainer()
        {
            if (ButterLibSubModule.Instance.GetServiceProvider() is null!)
            {
                foreach (var format in ButterLibSubModule.Instance.GetTempServiceProvider().GetRequiredService<IEnumerable<ISettingsFormat>>())
                foreach (var extension in format.Extensions)
                {
                    AvailableSettingsFormats[extension] = format;
                }
            }

            if (Settings == null)
            {
                Settings = new List<GlobalSettings>();

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
                Settings.AddRange(mbOptionScreenSettings);
            }

            foreach (var setting in Settings)
                RegisterSettings(setting);
        }
    }
}