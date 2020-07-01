using MCM.Abstractions.Attributes;
using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Containers;
using MCM.Abstractions.Settings.Containers.Global;
using MCM.Abstractions.Settings.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MCM.Implementation.Settings.Containers.Global
{
    [Version("e1.0.0",  3)]
    [Version("e1.0.1",  3)]
    [Version("e1.0.2",  3)]
    [Version("e1.0.3",  3)]
    [Version("e1.0.4",  3)]
    [Version("e1.0.5",  3)]
    [Version("e1.0.6",  3)]
    [Version("e1.0.7",  3)]
    [Version("e1.0.8",  3)]
    [Version("e1.0.9",  3)]
    [Version("e1.0.10", 3)]
    [Version("e1.0.11", 3)]
    [Version("e1.1.0",  3)]
    [Version("e1.2.0",  3)]
    [Version("e1.2.1",  3)]
    [Version("e1.3.0",  3)]
    [Version("e1.3.1",  3)]
    [Version("e1.4.0",  3)]
    [Version("e1.4.1",  3)]
    public class FluentGlobalSettingsContainer : BaseSettingsContainer<FluentGlobalSettings>, IFluentGlobalSettingsContainer
    {
        protected override string RootFolder { get; }

        public override List<SettingsDefinition> CreateModSettingsDefinitions
        {
            get
            {
                ReloadAll();

                return LoadedSettings.Keys
                    .Select(id => new SettingsDefinition(id))
                    .OrderByDescending(a => a.DisplayName)
                    .ToList();
            }
        }

        public FluentGlobalSettingsContainer()
        {
            RootFolder = Path.Combine(base.RootFolder, "Global");
        }

        private void ReloadAll()
        {
            if (AppDomain.CurrentDomain.GetData(FluentGlobalSettings.ContainerId) == null)
                AppDomain.CurrentDomain.SetData(FluentGlobalSettings.ContainerId, new Dictionary<string, FluentGlobalSettings>());

            var storage = (AppDomain.CurrentDomain.GetData(FluentGlobalSettings.ContainerId) as Dictionary<string, FluentGlobalSettings>)!;

            foreach (var fluentGlobalSettings in storage)
            {
                if (!LoadedSettings.ContainsKey(fluentGlobalSettings.Key))
                    RegisterSettings(fluentGlobalSettings.Value);
            }
        }
    }
}