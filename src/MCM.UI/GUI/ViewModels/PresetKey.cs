using MCM.Abstractions;

using TaleWorlds.Localization;

namespace MCM.UI.GUI.ViewModels
{
    internal sealed record PresetKey
    {
        public string Id { get; init; }
        public string Name { get; init; }
            
        public PresetKey(ISettingsPreset preset)
        {
            Id = preset.Id;
            Name = new TextObject(preset.Name).ToString();
        }
        public PresetKey(string id, string name)
        {
            Id = id;
            Name = new TextObject(name).ToString();
        }

        /// <inheritdoc />
        public override string ToString() => Name;

        /// <inheritdoc />
        public override int GetHashCode() => Id.GetHashCode();

        public bool Equals(PresetKey? other) => Id.Equals(other?.Id);
    }
}