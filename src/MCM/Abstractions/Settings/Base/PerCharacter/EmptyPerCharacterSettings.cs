using MCM.Abstractions.Settings.Properties;

namespace MCM.Abstractions.Settings.Base.PerCharacter
{
    public sealed class EmptyPerCharacterSettings : PerCharacterSettings<EmptyPerCharacterSettings>
    {
        /// <inheritdoc/>
        public override string Id => "empty_perchar_v1";
        /// <inheritdoc/>
        public override string DisplayName => "Empty PerCharacter Settings";
        /// <inheritdoc/>
        public override string Format => "memory";

        /// <inheritdoc/>
        protected override ISettingsPropertyDiscoverer? Discoverer => null;
    }
}