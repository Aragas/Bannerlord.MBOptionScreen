namespace MCM.Abstractions.Settings.Base.PerSave
{
    public sealed class EmptyPerSaveSettings : PerSaveSettings<EmptyPerSaveSettings>
    {
        /// <inheritdoc/>
        public override string Id => "empty_persave_v1";
        /// <inheritdoc/>
        public override string DisplayName => "Empty PerSave Settings";
    }
}