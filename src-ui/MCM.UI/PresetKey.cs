using MCM.Abstractions;

using TaleWorlds.Localization;

namespace MCM.UI;

internal sealed record PresetKey
{
    public string Id { get; init; }
    private readonly string _name;
    public string Name => new TextObject(_name).ToString();

    public PresetKey(ISettingsPreset preset)
    {
        Id = preset.Id;
        _name = preset.Name;
    }
    public PresetKey(string id, string name)
    {
        Id = id;
        _name = name;
    }

    /// <inheritdoc />
    public override string ToString() => Name;

    /// <inheritdoc />
    public override int GetHashCode() => Id.GetHashCode();

    public bool Equals(PresetKey? other) => Id.Equals(other?.Id);
}