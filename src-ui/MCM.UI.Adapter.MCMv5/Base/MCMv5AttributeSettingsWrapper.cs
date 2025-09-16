using MCM.Abstractions;
using MCM.Abstractions.Base;
using MCM.Abstractions.Base.Global;

using MCM.UI.Adapter.MCMv5.Presets;

namespace MCM.UI.Adapter.MCMv5.Base;

internal sealed class MCMv5AttributeSettingsWrapper : SettingsWrapper
{
    public override string DiscoveryType => "mcm_v5_attributes";

    public MCMv5AttributeSettingsWrapper(object? @object) : base(@object) { }

    protected override BaseSettings Create(object? @object) => new MCMv5AttributeSettingsWrapper(@object);
    protected override ISettingsPreset CreatePreset(object? @object) => new MCMv5AttributeSettingsPresetWrapper(@object);
}