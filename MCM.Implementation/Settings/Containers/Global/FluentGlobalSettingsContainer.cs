using MCM.Abstractions.Attributes;
using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Containers;
using MCM.Abstractions.Settings.Containers.Global;

using System;
using System.Collections.Generic;
using System.IO;

namespace MCM.Implementation.Settings.Containers.Global
{
    [Version("e1.0.0",  2)]
    [Version("e1.0.1",  2)]
    [Version("e1.0.2",  2)]
    [Version("e1.0.3",  2)]
    [Version("e1.0.4",  2)]
    [Version("e1.0.5",  2)]
    [Version("e1.0.6",  2)]
    [Version("e1.0.7",  2)]
    [Version("e1.0.8",  2)]
    [Version("e1.0.9",  2)]
    [Version("e1.0.10", 1)]
    [Version("e1.0.11", 1)]
    [Version("e1.1.0",  2)]
    [Version("e1.2.0",  2)]
    [Version("e1.2.1",  2)]
    [Version("e1.3.0",  2)]
    [Version("e1.3.1",  2)]
    [Version("e1.4.0",  2)]
    [Version("e1.4.1",  2)]
    public class FluentGlobalSettingsContainer : BaseSettingsContainer<FluentGlobalSettings>, IFluentGlobalSettingsContainer
    {
        protected override string RootFolder { get; }
        protected override Dictionary<string, FluentGlobalSettings> LoadedSettings
        {
            get
            {
                if (AppDomain.CurrentDomain.GetData(FluentGlobalSettings.ContainerId) == null)
                    AppDomain.CurrentDomain.SetData(FluentGlobalSettings.ContainerId, new Dictionary<string, FluentGlobalSettings>());
                return (AppDomain.CurrentDomain.GetData(FluentGlobalSettings.ContainerId) as Dictionary<string, FluentGlobalSettings>)!;
            }
        }

        public FluentGlobalSettingsContainer()
        {
            RootFolder = Path.Combine(base.RootFolder, "Global");
        }
    }
}