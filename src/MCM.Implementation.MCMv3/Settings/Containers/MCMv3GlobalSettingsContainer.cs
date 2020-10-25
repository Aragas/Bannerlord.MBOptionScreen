extern alias v3;
extern alias v4;

using Bannerlord.ButterLib.Common.Extensions;

using MCM.Implementation.MCMv3.Settings.Base;
using MCM.Implementation.MCMv3.Utils;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using v4::MCM.Abstractions.Settings.Base.Global;
using v4::MCM.Abstractions.Settings.Containers.Global;
using v4::MCM.Abstractions.Settings.Formats;

namespace MCM.Implementation.MCMv3.Settings.Containers
{
    // MCMv3 had the ability to get Instance in OnSubModuleLoad()
    internal sealed class MCMv3GlobalSettingsContainer : BaseGlobalSettingsContainer
    {
        private static List<GlobalSettings>? Settings { get; set; }

        public MCMv3GlobalSettingsContainer()
        {
            if (Settings == null)
            {
                Settings = new List<GlobalSettings>();

                var allTypes = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .SelectMany(a => a.GetTypes())
                    .Where(t => t.IsClass && !t.IsAbstract && t.GetConstructor(Type.EmptyTypes) != null)
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

        protected override void RegisterSettings(GlobalSettings settings)
        {
            if (settings == null || LoadedSettings.ContainsKey(settings.Id))
                return;

            LoadedSettings.Add(settings.Id, settings);

            var directoryPath = Path.Combine(RootFolder, settings.FolderName, settings.SubFolder);
            var serviceProvider = v4::MCM.MCMSubModule.Instance?.GetServiceProvider() ?? v4::MCM.MCMSubModule.Instance?.GetTempServiceProvider();
            var settingsFormats = serviceProvider.GetRequiredService<IEnumerable<ISettingsFormat>>() ?? Enumerable.Empty<ISettingsFormat>();
            var settingsFormat = settingsFormats.FirstOrDefault(x => x.FormatTypes.Any(y => y == settings.FormatType));
            settingsFormat?.Load(settings, directoryPath, settings.Id);
        }
    }
}