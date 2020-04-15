using MBOptionScreen.Attributes;

namespace MBOptionScreen.Settings.Wrapper
{
    /// <summary>
    /// Wrapper for SettingPropertyGroupAttribute. I think it world be better to make a model for it.
    /// </summary>
    internal sealed class SettingPropertyGroupAttributeWrapper : SettingPropertyGroupAttribute
    {
        private static string? GetGroupName(object @object)
        {
            var propInfo = @object.GetType().GetProperty("GroupName");
            return propInfo?.GetValue(@object) as string;
        }
        private static bool? GetIsMainToggle(object @object)
        {
            var propInfo = @object.GetType().GetProperty("IsMainToggle");
            return propInfo?.GetValue(@object) as bool?;
        }

        public SettingPropertyGroupAttributeWrapper(object @object) : base(
            GetGroupName(@object) ?? "ERROR",
            GetIsMainToggle(@object) ?? false)
        {
        }
    }
}