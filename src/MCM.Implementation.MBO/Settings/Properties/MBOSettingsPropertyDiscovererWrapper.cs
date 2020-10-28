namespace MCM.Implementation.MBO.Settings.Properties
{
    /// <summary>
    /// For DI
    /// </summary>
    public class MBOSettingsPropertyDiscovererWrapper : BaseSettingsPropertyDiscovererWrapper, IMBOSettingsPropertyDiscoverer
    {
        public MBOSettingsPropertyDiscovererWrapper(object @object) : base(@object) { }
    }
}