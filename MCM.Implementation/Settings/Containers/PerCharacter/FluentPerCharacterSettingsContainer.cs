using MCM.Abstractions.Attributes;
using MCM.Abstractions.Settings.Base.PerCharacter;
using MCM.Abstractions.Settings.Containers;
using MCM.Abstractions.Settings.Containers.PerCharacter;

using System;
using System.Collections.Generic;
using System.IO;

using TaleWorlds.Core;

namespace MCM.Implementation.Settings.Containers.PerCharacter
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
    public class FluentPerCharacterSettingsContainer : BaseSettingsContainer<FluentPerCharacterSettings>, IFluentPerCharacterSettingsContainer
    {
        protected override string RootFolder { get; }
        protected override Dictionary<string, FluentPerCharacterSettings> LoadedSettings
        {
            get
            {
                if (AppDomain.CurrentDomain.GetData(FluentPerCharacterSettings.ContainerId) == null)
                    AppDomain.CurrentDomain.SetData(FluentPerCharacterSettings.ContainerId, new Dictionary<string, FluentPerCharacterSettings>());
                return (AppDomain.CurrentDomain.GetData(FluentPerCharacterSettings.ContainerId) as Dictionary<string, FluentPerCharacterSettings>)!;
            }
        }

        public FluentPerCharacterSettingsContainer()
        {
            RootFolder = Path.Combine(base.RootFolder, "PerCharacter");
        }

        public void OnGameStarted(Game game) { LoadedSettings.Clear(); }
        public void OnGameEnded(Game game) { LoadedSettings.Clear(); }
    }
}