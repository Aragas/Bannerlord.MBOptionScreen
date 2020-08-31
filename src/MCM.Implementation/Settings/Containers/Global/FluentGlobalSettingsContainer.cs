using Bannerlord.ButterLib.Common.Extensions;

using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Containers;
using MCM.Abstractions.Settings.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MCM.Implementation.Settings.Containers.Global
{
    internal sealed class FluentGlobalSettingsContainer : BaseSettingsContainer<FluentGlobalSettings>, IMCMFluentGlobalSettingsContainer
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
            foreach (var (id, settings) in storage)
            {
                if (!LoadedSettings.ContainsKey(id))
                    RegisterSettings(settings);
            }
        }
    }
}