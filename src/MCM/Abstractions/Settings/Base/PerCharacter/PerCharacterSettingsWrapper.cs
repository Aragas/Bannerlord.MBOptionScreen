namespace MCM.Abstractions.Settings.Base.PerCharacter
{
    public sealed class PerCharacterSettingsWrapper : BasePerCharacterSettingsWrapper
    {
        public override string Id { get; }
        public override string DisplayName { get; }

        public PerCharacterSettingsWrapper(object @object) : base(@object) { }
    }
}