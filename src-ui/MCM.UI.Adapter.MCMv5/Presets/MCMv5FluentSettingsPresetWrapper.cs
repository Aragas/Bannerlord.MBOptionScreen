using MCM.Abstractions;

using MCM.UI.Adapter.MCMv5.Base;

namespace MCM.UI.Adapter.MCMv5.Presets
{
    internal sealed class MCMv5FluentSettingsPresetWrapper : SettingsPresetWrapper<MCMv5FluentSettingsWrapper>
    {
        public MCMv5FluentSettingsPresetWrapper(object? @object) : base(@object) { }
        protected override MCMv5FluentSettingsWrapper Create(object? @object) => new(@object);
    }
}