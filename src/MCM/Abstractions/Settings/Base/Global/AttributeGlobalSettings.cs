namespace MCM.Abstractions.Settings.Base.Global
{
    public abstract class AttributeGlobalSettings<T> : GlobalSettings<T> where T : GlobalSettings, new()
    {
        public override string DiscoveryType => "attributes";
    }
}