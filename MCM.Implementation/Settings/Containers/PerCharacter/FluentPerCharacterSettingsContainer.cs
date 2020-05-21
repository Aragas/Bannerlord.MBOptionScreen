using MCM.Abstractions.Attributes;
using MCM.Abstractions.Settings.Base.PerCharacter;
using MCM.Abstractions.Settings.Containers;
using MCM.Abstractions.Settings.Containers.PerCharacter;

using System.IO;

using TaleWorlds.Core;

namespace MCM.Implementation.Settings.Containers.PerCharacter
{
    [Version("e1.0.0",  1)]
    [Version("e1.0.1",  1)]
    [Version("e1.0.2",  1)]
    [Version("e1.0.3",  1)]
    [Version("e1.0.4",  1)]
    [Version("e1.0.5",  1)]
    [Version("e1.0.6",  1)]
    [Version("e1.0.7",  1)]
    [Version("e1.0.8",  1)]
    [Version("e1.0.9",  1)]
    [Version("e1.0.10", 1)]
    [Version("e1.0.11", 1)]
    [Version("e1.1.0",  1)]
    [Version("e1.2.0",  1)]
    [Version("e1.2.1",  1)]
    [Version("e1.3.0",  1)]
    [Version("e1.3.1",  1)]
    [Version("e1.4.0",  1)]
    [Version("e1.4.1",  1)]
    public class FluentPerCharacterSettingsContainer : BaseSettingsContainer<FluentPerCharacterSettings>, IFluentPerCharacterSettingsContainer
    {
        protected override string RootFolder { get; }

        public FluentPerCharacterSettingsContainer()
        {
            RootFolder = Path.Combine(base.RootFolder, "PerCharacter");
        }

        public void OnGameStarted(Game game) { LoadedSettings.Clear(); }
        public void OnGameEnded(Game game) { LoadedSettings.Clear(); }

        public void Register(FluentPerCharacterSettings settings) => LoadedSettings.Add(settings.Id, settings);
        public void Unregister(FluentPerCharacterSettings settings) => LoadedSettings.Remove(settings.Id);
    }
}