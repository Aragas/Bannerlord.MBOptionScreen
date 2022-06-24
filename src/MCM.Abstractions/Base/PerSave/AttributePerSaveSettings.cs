namespace MCM.Abstractions.Base.PerSave
{
    public abstract class AttributePerSaveSettings<T> : PerSaveSettings<T> where T : PerSaveSettings, new()
    {
        public sealed override string DiscoveryType => "attributes";
    }
}