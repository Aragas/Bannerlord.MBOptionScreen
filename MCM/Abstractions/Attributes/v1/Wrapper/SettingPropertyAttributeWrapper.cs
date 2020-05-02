namespace MCM.Abstractions.Attributes.v1.Wrapper
{
    /// <summary>
    /// Wrapper for SettingPropertyAttribute. I think it world be better to make a model for it.
    /// </summary>
    public sealed class SettingPropertyAttributeWrapper : SettingPropertyAttribute
    {
        private static string? GetDisplayName(object @object)
        {
            var propInfo = @object.GetType().GetProperty(nameof(DisplayName));
            return propInfo?.GetValue(@object) as string;
        }
        private static float? GetMinValue(object @object)
        {
            var propInfo = @object.GetType().GetProperty(nameof(MinValue));
            return propInfo?.GetValue(@object) as float?;
        }
        private static float? GetMaxValue(object @object)
        {
            var propInfo = @object.GetType().GetProperty(nameof(MaxValue));
            return propInfo?.GetValue(@object) as float?;
        }
        private static float? GetEditableMinValue(object @object)
        {
            var propInfo = @object.GetType().GetProperty(nameof(EditableMinValue));
            return propInfo?.GetValue(@object) as float?;
        }
        private static float? GetEditableMaxValue(object @object)
        {
            var propInfo = @object.GetType().GetProperty(nameof(EditableMaxValue));
            return propInfo?.GetValue(@object) as float?;
        }
        private static bool? GetRequireRestart(object @object)
        {
            var propInfo = @object.GetType().GetProperty(nameof(RequireRestart));
            return propInfo?.GetValue(@object) as bool?;
        }
        private static string? GetHintText(object @object)
        {
            var propInfo = @object.GetType().GetProperty(nameof(HintText));
            return propInfo?.GetValue(@object) as string;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Redundancy", "CS0618")]
        public SettingPropertyAttributeWrapper(object @object) : base(
            GetDisplayName(@object) ?? "ERROR",
            GetMinValue(@object) ?? 0f,
            GetMaxValue(@object) ?? 0f,
            GetEditableMinValue(@object) ?? 0f,
            GetEditableMaxValue(@object) ?? 0f,
            GetRequireRestart(@object) ?? true,
            GetHintText(@object) ?? "") { }
    }
}