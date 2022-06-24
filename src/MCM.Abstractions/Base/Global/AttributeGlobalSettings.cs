namespace MCM.Abstractions.Base.Global
{
    public abstract class AttributeGlobalSettings<T> : GlobalSettings<T> where T : GlobalSettings, new()
    {
        public sealed override string DiscoveryType => "attributes";
    }
}