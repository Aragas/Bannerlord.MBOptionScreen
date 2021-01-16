using MCM.Abstractions.Settings.Base.Global;

namespace MCM.Abstractions.Settings.Containers.Global
{
    public abstract class FluentGlobalSettingsContainerWrapper : BaseSettingsContainerWrapper, IFluentGlobalSettingsContainer
    {
        public override bool IsCorrect { get; }

        protected FluentGlobalSettingsContainerWrapper(object @object) : base(@object) { }

        public void Register(FluentGlobalSettings settings) { }
        public void Unregister(FluentGlobalSettings settings) { }
    }
}