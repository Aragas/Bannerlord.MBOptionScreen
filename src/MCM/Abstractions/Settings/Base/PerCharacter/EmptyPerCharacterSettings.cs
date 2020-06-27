using MCM.Abstractions.Settings.Properties;

namespace MCM.Abstractions.Settings.Base.PerCharacter
{
    public sealed class EmptyPerCharacterSettings : PerCharacterSettings<EmptyPerCharacterSettings>
    {
        public override string Id => "empty_perchar_v1";
        public override string DisplayName => "Empty PerCharacter Settings";
        public override string Format => "memory";

        protected override ISettingsPropertyDiscoverer? Discoverer => null;
    }
}