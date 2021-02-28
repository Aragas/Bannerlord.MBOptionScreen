extern alias v3;
extern alias v4;

using MCM.Adapter.MCMv3.Settings.Base;
using MCM.Adapter.MCMv3.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using v4::MCM.Abstractions.Settings.Base.Global;
using v4::MCM.Abstractions.Settings.Containers.Global;
using v4::MCM.Abstractions.Settings.Formats;
using v4::MCM.DependencyInjection;

namespace MCM.Adapter.MCMv3.Settings.Containers
{
    // MCMv3 had the ability to get Instance in OnSubModuleLoad()
    internal sealed class MCMv3GlobalSettingsContainer : BaseGlobalSettingsContainer
    {
        private static List<GlobalSettings>? Settings { get; set; }

        public MCMv3GlobalSettingsContainer()
        {
            if (Settings is null)
            {
                Settings = new List<GlobalSettings>();

                var allTypes = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .SelectMany(a => a.GetTypes())
                    .Where(t => t.IsClass && !t.IsAbstract && t.GetConstructor(Type.EmptyTypes) is not null)
                    .ToList();

                var mbOptionScreenSettings = allTypes
                    .Where(t => typeof(v3::MCM.Abstractions.Settings.Base.Global.GlobalSettings).IsAssignableFrom(t))
                    .Where(t => !typeof(v3::MCM.Abstractions.Settings.Base.Global.EmptyGlobalSettings).IsAssignableFrom(t))
                    .Where(t => !ReflectionUtils.ImplementsOrImplementsEquivalent(t, "MCM.MCMSettings"))
                    .Where(t => !typeof(v3::MCM.Abstractions.IWrapper).IsAssignableFrom(t))
                    .Select(obj => Activator.CreateInstance(obj) is { } val ? new MCMv3GlobalSettingsWrapper(val) : null)
                    .Where(t => t is not null)
                    .Cast<GlobalSettings>();
                Settings.AddRange(mbOptionScreenSettings);
            }

            foreach (var setting in Settings)
                RegisterSettings(setting);
        }

        protected override void RegisterSettings(GlobalSettings? settings)
        {
            if (settings is null || LoadedSettings.ContainsKey(settings.Id))
                return;

            LoadedSettings.Add(settings.Id, settings);

            var directoryPath = Path.Combine(RootFolder, settings.FolderName, settings.SubFolder);
            var settingsFormats = GenericServiceProvider.GetService<IEnumerable<ISettingsFormat>>() ?? Enumerable.Empty<ISettingsFormat>();
            var settingsFormat = settingsFormats.FirstOrDefault(x => x.FormatTypes.Any(y => y == settings.FormatType));
            settingsFormat?.Load(settings, directoryPath, settings.Id);
        }
    }
}