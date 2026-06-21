using MCM.Abstractions;

using MCM.UI.Adapter.MCMv5.Base;

namespace MCM.UI.Adapter.MCMv5.Presets
{
    internal sealed class MCMv5AttributeSettingsPresetWrapper : SettingsPresetWrapper<MCMv5AttributeSettingsWrapper>
    {
        public MCMv5AttributeSettingsPresetWrapper(object? @object) : base(@object) { }
        protected override MCMv5AttributeSettingsWrapper Create(object? @object) => new(@object);
    }
}