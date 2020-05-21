namespace MCM.Abstractions.Settings.Containers.Global
{
    public abstract class BaseGlobalSettingsContainerWrapper : BaseSettingsContainerWrapper, IGlobalSettingsContainer
    {
        protected BaseGlobalSettingsContainerWrapper(object @object) : base(@object) { }
    }
}