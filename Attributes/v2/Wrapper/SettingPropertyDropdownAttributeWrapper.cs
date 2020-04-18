using MBOptionScreen.Attributes;

using System.Reflection;

namespace MBOptionScreen.Settings.Wrapper
{
    internal sealed class SettingPropertyDropdownAttributeWrapper : SettingPropertyDropdownAttribute
    {
        private static string? GetDisplayName(object @object)
        {
            var propInfo = @object.GetType().GetProperty("DisplayName", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return propInfo?.GetValue(@object) as string;
        }
        private static int? GetSelectedIndex(object @object)
        {
            var propInfo = @object.GetType().GetProperty("SelectedIndex", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return propInfo?.GetValue(@object) as int?;
        }
        private static int? GetOrder(object @object)
        {
            var propInfo = @object.GetType().GetProperty("Order", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return propInfo?.GetValue(@object) as int?;
        }
        private static bool? GetRequireRestart(object @object)
        {
            var propInfo = @object.GetType().GetProperty("RequireRestart", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return propInfo?.GetValue(@object) as bool?;
        }
        private static string? GetHintText(object @object)
        {
            var propInfo = @object.GetType().GetProperty("HintText", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return propInfo?.GetValue(@object) as string;
        }

        public SettingPropertyDropdownAttributeWrapper(object @object) : base(
            GetDisplayName(@object) ?? "ERROR",
            GetSelectedIndex(@object) ?? 0,
            GetOrder(@object) ?? -1,
            GetRequireRestart(@object) ?? true,
            GetHintText(@object) ?? "") { }
    }
}