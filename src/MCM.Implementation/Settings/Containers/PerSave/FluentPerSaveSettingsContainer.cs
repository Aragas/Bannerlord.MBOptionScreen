using Bannerlord.ButterLib.Common.Extensions;

using MCM.Abstractions.Settings.Base.PerSave;
using MCM.Abstractions.Settings.Containers;
using MCM.Abstractions.Settings.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using TaleWorlds.Core;

namespace MCM.Implementation.Settings.Containers.PerSave
{
    internal sealed class FluentPerSaveSettingsContainer : BaseSettingsContainer<FluentPerSaveSettings>, IMCMFluentPerSaveSettingsContainer
    {
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

        private void ReloadAll()
        {
            if (AppDomain.CurrentDomain.GetData(FluentPerSaveSettings.ContainerId) == null)
                AppDomain.CurrentDomain.SetData(FluentPerSaveSettings.ContainerId, new Dictionary<string, FluentPerSaveSettings>());

            var storage = (AppDomain.CurrentDomain.GetData(FluentPerSaveSettings.ContainerId) as Dictionary<string, FluentPerSaveSettings>)!;
            foreach (var (id, settings) in storage)
            {
                if (!LoadedSettings.ContainsKey(id))
                    RegisterSettings(settings);
            }
        }

        public void OnGameStarted(Game game) => LoadedSettings.Clear();
        public void OnGameEnded(Game game) => LoadedSettings.Clear();
    }
}