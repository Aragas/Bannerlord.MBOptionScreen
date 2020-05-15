using MCM.Abstractions.Settings.SettingsContainer;

namespace MCM.Implementation.MBO.Settings.SettingsContainer
{
    /// <summary>
    /// For DI
    /// </summary>
    public sealed class MBOGlobalSettingsContainerWrapper : BaseSettingsContainerWrapper, IMBOGlobalSettingsContainer
    {
        public MBOGlobalSettingsContainerWrapper(object @object) : base(@object) { }
    }
}