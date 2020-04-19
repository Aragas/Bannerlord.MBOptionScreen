using MBOptionScreen.Attributes;

using System.Reflection;

namespace MBOptionScreen.Settings.Wrapper
{
    internal sealed class SettingPropertyIntegerAttributeWrapper : SettingPropertyIntegerAttribute
    {
        private static string? GetDisplayName(object @object)
        {
            var propInfo = @object.GetType().GetProperty("DisplayName", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return propInfo?.GetValue(@object) as string;
        }
        private static int? GetMinValue(object @object)
        {
            var propInfo = @object.GetType().GetProperty("MinValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return propInfo?.GetValue(@object) as int?;
        }
        private static int? GetMaxValue(object @object)
        {
            var propInfo = @object.GetType().GetProperty("MaxValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
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
        private static string? GetValueFormat(object @object)
        {
            var propInfo = @object.GetType().GetProperty("ValueFormat", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return propInfo?.GetValue(@object) as string;
        }

        public SettingPropertyIntegerAttributeWrapper(object @object) : base(
            GetDisplayName(@object) ?? "ERROR",
            GetMinValue(@object) ?? 0,
            GetMaxValue(@object) ?? 0,
            GetOrder(@object) ?? -1,
            GetRequireRestart(@object) ?? true,
            GetHintText(@object) ?? "",
            GetValueFormat(@object) ?? "") { }
    }
}