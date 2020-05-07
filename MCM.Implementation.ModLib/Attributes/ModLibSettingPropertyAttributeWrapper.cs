using MCM.Abstractions.Attributes.v1;

namespace MCM.Implementation.ModLib.Attributes
{
    public sealed class ModLibSettingPropertyAttributeWrapper : SettingPropertyAttribute
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

        /// <summary>
        /// The absolute minimum value that this setting can be set to. This is used for the editing dialog. Set this if you wish users to be able to set values outside your recommended values.
        /// </summary>
        public float EditableMinValue { get; } = 0f;
        /// <summary>
        /// The absolute maximum value that this setting can be set to. This is used for the editing dialog. Set this if you wish users to be able to set values outside your recommended values.
        /// </summary>
        public float EditableMaxValue { get; } = 0f;

        public ModLibSettingPropertyAttributeWrapper(object @object) : base(
            GetDisplayName(@object) ?? "ERROR",
            GetMinValue(@object) ?? 0f,
            GetMaxValue(@object) ?? 0f)
        {
            EditableMinValue = GetEditableMinValue(@object) ?? 0f;
            EditableMaxValue = GetEditableMaxValue(@object) ?? 0f;
            RequireRestart = GetRequireRestart(@object) ?? true;
            HintText = GetHintText(@object) ?? "";
        }
    }
}