using MCM.Abstractions.Settings.Properties;

namespace MCM.Abstractions.Settings.Base.Global
{
    public sealed class EmptyGlobalSettings : GlobalSettings<EmptyGlobalSettings>
    {
        /// <inheritdoc/>
        public override string Id => "empty_v1";
        /// <inheritdoc/>
        public override string DisplayName => "Empty Global Settings";
        /// <inheritdoc/>
        public override string Format => "memory";

        /// <inheritdoc/>
        protected override ISettingsPropertyDiscoverer? Discoverer => null;
    }
}