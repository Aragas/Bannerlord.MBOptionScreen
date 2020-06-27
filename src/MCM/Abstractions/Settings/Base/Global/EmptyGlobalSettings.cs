using MCM.Abstractions.Settings.Properties;

namespace MCM.Abstractions.Settings.Base.Global
{
    public sealed class EmptyGlobalSettings : GlobalSettings<EmptyGlobalSettings>
    {
        public override string Id => "empty_v1";
        public override string DisplayName => "Empty Global Settings";
        public override string Format => "memory";

        protected override ISettingsPropertyDiscoverer? Discoverer => null;
    }
}