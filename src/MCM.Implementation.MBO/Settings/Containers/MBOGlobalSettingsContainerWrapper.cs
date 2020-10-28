namespace MCM.Implementation.MBO.Settings.Containers
{
    /// <summary>
    /// For DI
    /// </summary>
    public sealed class MBOGlobalSettingsContainerWrapper : BaseSettingsContainerWrapper, IMBOGlobalSettingsContainer
    {
        public MBOGlobalSettingsContainerWrapper(object @object) : base(@object) { }
    }
}