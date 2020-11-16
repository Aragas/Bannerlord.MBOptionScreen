extern alias v1;
extern alias v4;

using Bannerlord.ButterLib.Common.Extensions;

using MCM.Adapter.MBO.Settings.Base;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using v4::MCM.Abstractions.Settings.Base.Global;
using v4::MCM.Abstractions.Settings.Containers.Global;
using v4::MCM.Abstractions.Settings.Formats;

using SettingsBase = v1::MBOptionScreen.Settings.SettingsBase;

namespace MCM.Adapter.MBO.Settings.Containers
{
    internal sealed class MBOv1GlobalSettingsContainer : BaseGlobalSettingsContainer
    {
        private static List<GlobalSettings>? Settings { get; set; }

        public MBOv1GlobalSettingsContainer()
        {
            if (Settings is null)
            {
                Settings = new List<GlobalSettings>();

                var allTypes = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    // ignore v1 and v2 classes
                    .Where(a => !Path.GetFileNameWithoutExtension(a.Location).StartsWith("MBOptionScreen"))
                    .SelectMany(a => a.GetTypes())
                    .Where(t => t.IsClass && !t.IsAbstract)
                    .Where(t => t.GetConstructor(Type.EmptyTypes) is not null)
                    .ToList();

                var mbOptionScreenSettings = allTypes
                    .Where(t => typeof(SettingsBase).IsAssignableFrom(t))
                    .Select(obj => Activator.CreateInstance(obj) is { } val ? new MBOv1GlobalSettingsWrapper(val) : null)
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
            var serviceProvider = v4::MCM.MCMSubModule.Instance?.GetServiceProvider() ?? v4::MCM.MCMSubModule.Instance?.GetTempServiceProvider();
            var settingsFormats = serviceProvider.GetRequiredService<IEnumerable<ISettingsFormat>>() ?? Enumerable.Empty<ISettingsFormat>();
            var settingsFormat = settingsFormats.FirstOrDefault(x => x.FormatTypes.Any(y => y == settings.FormatType));
            settingsFormat?.Load(settings, directoryPath, settings.Id);
        }
    }
}