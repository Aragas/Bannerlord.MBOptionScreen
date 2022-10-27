namespace MCM.Abstractions.Wrapper
{
    public sealed class SettingsDefinitionWrapper : SettingsDefinition
    {
        private static string? GetSettingsId(object @object)
        {
            var propInfo = @object.GetType().GetProperty(nameof(SettingsId));
            return propInfo?.GetValue(@object) as string;
        }

        public SettingsDefinitionWrapper(object @object) : base(GetSettingsId(@object) ?? "ERROR") { }
    }
}