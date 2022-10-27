
namespace MCM.Abstractions.Base.Global
{
    public sealed class EmptyGlobalSettings : GlobalSettings<EmptyGlobalSettings>
    {
        /// <inheritdoc/>
        public override string Id => "empty_v1";
        /// <inheritdoc/>
        public override string DisplayName => "Empty Global Settings";
    }
}