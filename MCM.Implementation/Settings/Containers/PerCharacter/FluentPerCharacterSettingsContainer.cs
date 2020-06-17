using MCM.Abstractions.Attributes;
using MCM.Abstractions.Settings.Base.PerCharacter;
using MCM.Abstractions.Settings.Containers;
using MCM.Abstractions.Settings.Containers.PerCharacter;
using MCM.Abstractions.Settings.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using TaleWorlds.Core;

namespace MCM.Implementation.Settings.Containers.PerCharacter
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
    public class FluentPerCharacterSettingsContainer : BaseSettingsContainer<FluentPerCharacterSettings>, IFluentPerCharacterSettingsContainer
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

        public FluentPerCharacterSettingsContainer()
        {
            RootFolder = Path.Combine(base.RootFolder, "PerCharacter");
        }

        private void ReloadAll()
        {
            if (AppDomain.CurrentDomain.GetData(FluentPerCharacterSettings.ContainerId) == null)
                AppDomain.CurrentDomain.SetData(FluentPerCharacterSettings.ContainerId, new Dictionary<string, FluentPerCharacterSettings>());

            var storage = (AppDomain.CurrentDomain.GetData(FluentPerCharacterSettings.ContainerId) as Dictionary<string, FluentPerCharacterSettings>)!;

            foreach (var fluentGlobalSettings in storage)
            {
                if (!LoadedSettings.ContainsKey(fluentGlobalSettings.Key))
                    RegisterSettings(fluentGlobalSettings.Value);
            }

        }

        public void OnGameStarted(Game game) { LoadedSettings.Clear(); }
        public void OnGameEnded(Game game) { LoadedSettings.Clear(); }
    }
}