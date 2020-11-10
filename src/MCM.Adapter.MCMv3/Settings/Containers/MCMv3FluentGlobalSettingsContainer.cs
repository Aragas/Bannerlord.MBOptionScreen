extern alias v3;
extern alias v4;

using Bannerlord.ButterLib.Common.Extensions;

using MCM.Adapter.MCMv3.Settings.Base;

using System;
using System.Collections.Generic;
using System.Linq;

using v4::MCM.Abstractions.Settings.Base.Global;
using v4::MCM.Abstractions.Settings.Containers.Global;
using v4::MCM.Abstractions.Settings.Models;

using LegacyFluentGlobalSettings = v3::MCM.Abstractions.Settings.Base.Global.FluentGlobalSettings;

namespace MCM.Adapter.MCMv3.Settings.Containers
{
    internal sealed class MCMv3FluentGlobalSettingsContainer : v4::MCM.Abstractions.Settings.Containers.BaseSettingsContainer<FluentGlobalSettings>, IFluentGlobalSettingsContainer
    {
        private void ReloadAll()
        {
            var containerId = LegacyFluentGlobalSettings.ContainerId;
            if (AppDomain.CurrentDomain.GetData(containerId) is null)
                AppDomain.CurrentDomain.SetData(containerId, new Dictionary<string, LegacyFluentGlobalSettings>());

            var storage = (AppDomain.CurrentDomain.GetData(containerId) as Dictionary<string, LegacyFluentGlobalSettings>)!;
            foreach (var (id, settings) in storage)
            {
                if (!LoadedSettings.ContainsKey(id))
                    RegisterSettings(new MCMv3FluentGlobalSettingsWrapper(settings));
            }
        }
        public override List<SettingsDefinition> CreateModSettingsDefinitions
        {
            get
            {
                ReloadAll();

                return LoadedSettings.Keys
                    .Select(id => new SettingsDefinition(id))
                    .ToList();
            }
        }

        public void Register(FluentGlobalSettings settings) { }
        public void Unregister(FluentGlobalSettings settings) { }
    }
}