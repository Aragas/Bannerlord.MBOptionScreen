extern alias v3;
extern alias v4;

using Bannerlord.ButterLib.Common.Extensions;

using MCM.Implementation.MCMv3.Settings.Base;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using v4::MCM.Abstractions.Settings.Base.Global;
using v4::MCM.Abstractions.Settings.Containers;
using v4::MCM.Abstractions.Settings.Models;

using LegacyFluentGlobalSettings = v3::MCM.Abstractions.Settings.Base.Global.FluentGlobalSettings;

namespace MCM.Implementation.MCMv3.Settings.Containers
{
    internal class MCMv3FluentGlobalSettingsContainer : BaseSettingsContainer<FluentGlobalSettings>, IMCMv3FluentGlobalSettingsContainer
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

        public MCMv3FluentGlobalSettingsContainer()
        {
            RootFolder = Path.Combine(base.RootFolder, "Global");
        }

        private void ReloadAll()
        {
            var containerId = LegacyFluentGlobalSettings.ContainerId;
            if (AppDomain.CurrentDomain.GetData(containerId) == null)
                AppDomain.CurrentDomain.SetData(containerId, new Dictionary<string, LegacyFluentGlobalSettings>());

            var storage = (AppDomain.CurrentDomain.GetData(containerId) as Dictionary<string, LegacyFluentGlobalSettings>)!;
            foreach (var (id, settings) in storage)
            {
                if (!LoadedSettings.ContainsKey(id))
                    RegisterSettings(new MCMv3FluentGlobalSettingsWrapper(settings));
            }
        }
    }
}