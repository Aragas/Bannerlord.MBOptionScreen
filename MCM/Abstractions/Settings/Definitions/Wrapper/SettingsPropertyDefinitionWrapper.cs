using HarmonyLib;

using MCM.Utils;

using System;
using System.Reflection;

using TaleWorlds.Localization;

namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class SettingsPropertyDefinitionWrapper : ISettingsPropertyDefinition
    {
        private PropertyInfo? SettingsIdProperty { get; }
        private PropertyInfo? SettingTypeProperty { get; }
        private PropertyInfo? PropertyProperty { get; }
        private PropertyInfo? DisplayNameProperty { get; }
        private PropertyInfo? HintTextProperty { get; }
        private PropertyInfo? OrderProperty { get; }
        private PropertyInfo? RequireRestartProperty { get; }
        private PropertyInfo? GroupNameProperty { get; }
        private PropertyInfo? IsMainToggleProperty { get; }
        private PropertyInfo? MinValueProperty { get; }
        private PropertyInfo? MaxValueProperty { get; }
        private PropertyInfo? EditableMinValueProperty { get; }
        private PropertyInfo? EditableMaxValueProperty { get; }
        private PropertyInfo? SelectedIndexProperty { get; }

        public string SettingsId { get; }
        public PropertyInfo Property { get; }
        public SettingType SettingType { get; }
        public string DisplayName { get; }
        public int Order { get; } = -1;
        public bool RequireRestart { get; } = true;
        public string HintText { get; } //= new TextObject("");
        public decimal MaxValue { get; } = 0m;
        public decimal MinValue { get; } = 0m;
        public decimal EditableMinValue { get; } = 0m;
        public decimal EditableMaxValue { get; } = 0m;
        public int SelectedIndex { get; } = 0;
        public string ValueFormat { get; } = "";
        public string GroupName { get; } = SettingsPropertyGroupDefinition.DefaultGroupName;
        public bool IsMainToggle { get; } = false;
        public int GroupOrder { get; } = -1;

        public SettingsPropertyDefinitionWrapper(object @object) : base()
        {
            var type = @object.GetType();

            SettingsIdProperty = AccessTools.Property(type, nameof(SettingsId));
            SettingTypeProperty = AccessTools.Property(type, nameof(SettingType));
            PropertyProperty = AccessTools.Property(type, nameof(Property));
            DisplayNameProperty = AccessTools.Property(type, nameof(DisplayName));
            HintTextProperty = AccessTools.Property(type, nameof(HintText));
            OrderProperty = AccessTools.Property(type, nameof(Order));
            RequireRestartProperty = AccessTools.Property(type, nameof(RequireRestart));
            GroupNameProperty = AccessTools.Property(type, nameof(GroupName));
            IsMainToggleProperty = AccessTools.Property(type, nameof(IsMainToggle));
            MinValueProperty = AccessTools.Property(type, nameof(MinValue));
            MaxValueProperty = AccessTools.Property(type, nameof(MaxValue));
            EditableMinValueProperty = AccessTools.Property(type, nameof(EditableMinValue));
            EditableMaxValueProperty = AccessTools.Property(type, nameof(EditableMaxValue));
            SelectedIndexProperty = AccessTools.Property(type, nameof(SelectedIndex));


            SettingsId = SettingsIdProperty?.GetValue(@object) as string ?? "ERROR";
            SettingType = SettingTypeProperty?.GetValue(@object) is { } settingTypeObject
                ? Enum.TryParse<SettingType>(settingTypeObject.ToString(), out var resultEnum) 
                    ? resultEnum
                    : SettingType.NONE
                : SettingType.NONE;
            Property = new WrapperPropertyInfo((PropertyInfo) PropertyProperty?.GetValue(@object));

            DisplayName = DisplayNameProperty?.GetValue(@object) switch
            {
                string str => str,
                TextObject to => to.ToString(),
                _ => DisplayName
            };
            HintText = HintTextProperty?.GetValue(@object) switch
            {
                string str => str,
                TextObject to => to.ToString(),
                _ => HintText
            };
            Order = OrderProperty?.GetValue(@object) as int? ?? -1;
            RequireRestart = RequireRestartProperty?.GetValue(@object) as bool? ?? true;

            GroupName = GroupNameProperty?.GetValue(@object) as string ?? "";
            IsMainToggle = IsMainToggleProperty?.GetValue(@object) as bool? ?? false;

            MinValue = MinValueProperty?.GetValue(@object) is { } minVal ? minVal as decimal? ?? 0 : 0;
            MaxValue = MaxValueProperty?.GetValue(@object) is { } maxVal ? maxVal as decimal? ?? 0 : 0;
            EditableMinValue = EditableMinValueProperty?.GetValue(@object) is { } eMinVal ? eMinVal as decimal? ?? 0 : 0;
            EditableMaxValue = EditableMaxValueProperty?.GetValue(@object) is { } eMaxValue ? eMaxValue as decimal? ?? 0 : 0;

            SelectedIndex = SelectedIndexProperty?.GetValue(@object) as int? ?? 0;
        }
    }
}