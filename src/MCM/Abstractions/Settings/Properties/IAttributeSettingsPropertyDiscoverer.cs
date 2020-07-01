namespace MCM.Abstractions.Settings.Properties
{
    /// <summary>
    /// So it can be overriden by an external library
    /// </summary>
    public interface IAttributeSettingsPropertyDiscoverer : ISettingsPropertyDiscoverer, IDependency { }
}