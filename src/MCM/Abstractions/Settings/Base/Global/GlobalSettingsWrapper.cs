namespace MCM.Abstractions.Settings.Base.Global
{
    public sealed class GlobalSettingsWrapper : BaseGlobalSettingsWrapper
    {
        public override string Id { get; }
        public override string DisplayName { get; }

        public GlobalSettingsWrapper(object @object) : base(@object) { }
    }
}