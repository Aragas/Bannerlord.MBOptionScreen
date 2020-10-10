namespace MCM.Abstractions.Settings.Base.PerSave
{
    public abstract class AttributePerSaveSettings<T> : PerSaveSettings<T> where T : PerSaveSettings, new()
    {
        public override string DiscoveryType => "attributes";
    }
}