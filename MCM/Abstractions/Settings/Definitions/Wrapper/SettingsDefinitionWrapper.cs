namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class SettingsDefinitionWrapper : SettingsDefinition
    {
        private static string? GetSettingsId(object @object)
        {
            var propInfo = @object.GetType().GetProperty(nameof(SettingsId));
            return propInfo?.GetValue(@object) as string;
        }

        private readonly object _object;

        public SettingsDefinitionWrapper(object @object) : base(GetSettingsId(@object) ?? "ERROR")
        {
            _object = @object;
            var type = @object.GetType();
        }
    }
}