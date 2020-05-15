namespace MCM.Abstractions.Settings.SettingsContainer
{
    public abstract class BaseGlobalSettingsContainerWrapper : BaseSettingsContainerWrapper, IGlobalSettingsContainer
    {
        protected BaseGlobalSettingsContainerWrapper(object @object) : base(@object) { }
    }
}