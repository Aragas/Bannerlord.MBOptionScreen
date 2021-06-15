extern alias v2;
extern alias v4;

using HarmonyLib.BUTR.Extensions;

using MCM.Adapter.MBO.Settings.Base;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using v4::BUTR.DependencyInjection;
using v4::MCM.Abstractions.Settings.Base.Global;
using v4::MCM.Abstractions.Settings.Containers.Global;
using v4::MCM.Abstractions.Settings.Formats;

namespace MCM.Adapter.MBO.Settings.Containers
{
    internal sealed class MBOv2GlobalSettingsContainer : BaseGlobalSettingsContainer
    {
        private static List<GlobalSettings>? Settings { get; set; }

        public MBOv2GlobalSettingsContainer()
        {
            if (Settings is null)
            {
                Settings = new List<GlobalSettings>();

                var allTypes = AccessTools2.AllAssemblies()
                    .Where(a => !a.IsDynamic)
                    // ignore v1 and v2 classes
                    .Where(a => !Path.GetFileNameWithoutExtension(a.Location).StartsWith("MBOptionScreen"))
                    .SelectMany(AccessTools2.GetTypesFromAssembly)
                    .Where(t => t.IsClass && !t.IsAbstract)
                    .Where(t => t.GetConstructor(Type.EmptyTypes) is not null)
                    .ToList();

                var mbOptionScreenSettings = allTypes
                    .Where(t => typeof(v2::MBOptionScreen.Settings.SettingsBase).IsAssignableFrom(t))
                    .Select(obj => Activator.CreateInstance(obj) is { } val ? new MBOv2GlobalSettingsWrapper(val) : null)
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