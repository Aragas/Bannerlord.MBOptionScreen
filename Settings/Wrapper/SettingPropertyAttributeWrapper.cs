using MBOptionScreen.Attributes;

namespace MBOptionScreen.Settings.Wrapper
{
    /// <summary>
    /// Wrapper for SettingPropertyAttribute. I think it world be better to make a model for it.
    /// </summary>
    internal sealed class SettingPropertyAttributeWrapper : SettingPropertyAttribute
    {
        private static string? GetDisplayName(object @object)
        {
            var propInfo = @object.GetType().GetProperty("DisplayName");
            return propInfo?.GetValue(@object) as string;
        }
        private static float? GetMinValue(object @object)
        {
            var propInfo = @object.GetType().GetProperty("MinValue");
            return propInfo?.GetValue(@object) as float?;
        }
        private static float? GetMaxValue(object @object)
        {
            var propInfo = @object.GetType().GetProperty("MaxValue");
            return propInfo?.GetValue(@object) as float?;
        }
        private static float? GetEditableMinValue(object @object)
        {
            var propInfo = @object.GetType().GetProperty("EditableMinValue");
            return propInfo?.GetValue(@object) as float?;
        }
        private static float? GetEditableMaxValue(object @object)
        {
            var propInfo = @object.GetType().GetProperty("EditableMaxValue");
            return propInfo?.GetValue(@object) as float?;
        }
        private static bool? GetRequireRestart(object @object)
        {
            var propInfo = @object.GetType().GetProperty("RequireRestart");
            return propInfo?.GetValue(@object) as bool?;
        }
        private static string? GetHintText(object @object)
        {
            var propInfo = @object.GetType().GetProperty("HintText");
            return propInfo?.GetValue(@object) as string;
        }


        public SettingPropertyAttributeWrapper(object @object) : base(
            GetDisplayName(@object) ?? "ERROR",
            GetMinValue(@object) ?? 0f,
            GetMaxValue(@object) ?? 0f,
            GetEditableMinValue(@object) ?? 0f,
            GetEditableMaxValue(@object) ?? 0f,
            GetRequireRestart(@object) ?? true,
            GetHintText(@object) ?? "")
        {
        }
    }
}