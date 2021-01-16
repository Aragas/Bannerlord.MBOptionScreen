using Bannerlord.BUTR.Shared.Helpers;

using HarmonyLib;

using MCM.Abstractions.Ref;

using System;
using System.Reflection;

using TaleWorlds.Localization;

namespace MCM.Abstractions.Settings.Models.Wrapper
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
        private PropertyInfo? GroupOrderProperty { get; }
        private PropertyInfo? IsMainToggleProperty { get; }
        private PropertyInfo? MinValueProperty { get; }
        private PropertyInfo? MaxValueProperty { get; }
        private PropertyInfo? EditableMinValueProperty { get; }
        private PropertyInfo? EditableMaxValueProperty { get; }
        private PropertyInfo? SelectedIndexProperty { get; }
        private PropertyInfo? ValueFormatProperty { get; }
        private PropertyInfo? CustomFormatterProperty { get; }
        private PropertyInfo? IdProperty { get; }

        public string SettingsId { get; }
        public IRef PropertyReference { get; }
        public SettingType SettingType { get; }
        public string DisplayName { get; }
        public int Order { get; }
        public bool RequireRestart { get; }
        public string HintText { get; }
        public decimal MaxValue { get; }
        public decimal MinValue { get; }
        public decimal EditableMinValue { get; }
        public decimal EditableMaxValue { get; }
        public int SelectedIndex { get; }
        public string ValueFormat { get; }
        public Type? CustomFormatter { get; }
        public string GroupName { get; }
        public bool IsMainToggle { get; }
        public int GroupOrder { get; }
        public string Id { get; }

        public SettingsPropertyDefinitionWrapper(object @object)
        {
            var type = @object.GetType();

            SettingsIdProperty = AccessTools.Property(type, nameof(SettingsId));
            SettingTypeProperty = AccessTools.Property(type, nameof(SettingType));
            PropertyProperty = AccessTools.Property(type, nameof(PropertyReference));
            DisplayNameProperty = AccessTools.Property(type, nameof(DisplayName));
            HintTextProperty = AccessTools.Property(type, nameof(HintText));
            OrderProperty = AccessTools.Property(type, nameof(Order));
            RequireRestartProperty = AccessTools.Property(type, nameof(RequireRestart));
            GroupNameProperty = AccessTools.Property(type, nameof(GroupName));
            GroupOrderProperty = AccessTools.Property(type, nameof(GroupOrder));
            IsMainToggleProperty = AccessTools.Property(type, nameof(IsMainToggle));
            MinValueProperty = AccessTools.Property(type, nameof(MinValue));
            MaxValueProperty = AccessTools.Property(type, nameof(MaxValue));
            EditableMinValueProperty = AccessTools.Property(type, nameof(EditableMinValue));
            EditableMaxValueProperty = AccessTools.Property(type, nameof(EditableMaxValue));
            SelectedIndexProperty = AccessTools.Property(type, nameof(SelectedIndex));
            ValueFormatProperty = AccessTools.Property(type, nameof(ValueFormat));
            CustomFormatterProperty = AccessTools.Property(type, nameof(CustomFormatter));
            IdProperty = AccessTools.Property(type, nameof(Id));


            SettingsId = SettingsIdProperty?.GetValue(@object) as string ?? "ERROR";
            SettingType = SettingTypeProperty?.GetValue(@object) is { } settingTypeObject
                ? Enum.TryParse<SettingType>(settingTypeObject.ToString(), out var resultEnum)
                    ? resultEnum
                    : SettingType.NONE
                : SettingType.NONE;
            PropertyReference = PropertyProperty?.GetValue(@object) is { } value
                ? value is IRef @ref ? @ref : new RefWrapper(value)
                : new ProxyRef<object?>(() => null, o => { });

            DisplayName = DisplayNameProperty?.GetValue(@object) switch
            {
                string str => TextObjectHelper.Create(str).ToString(),
                TextObject to => to.ToString(),
                _ => DisplayName
            };
            HintText = HintTextProperty?.GetValue(@object) switch
            {
                string str => TextObjectHelper.Create(str).ToString(),
                TextObject to => to.ToString(),
                _ => HintText
            };
            Order = OrderProperty?.GetValue(@object) as int? ?? -1;
            RequireRestart = RequireRestartProperty?.GetValue(@object) as bool? ?? true;

            GroupName = TextObjectHelper.Create(GroupNameProperty?.GetValue(@object) as string ?? "").ToString();
            GroupOrder = GroupOrderProperty?.GetValue(@object) as int? ?? -1;
            IsMainToggle = IsMainToggleProperty?.GetValue(@object) as bool? ?? false;

            MinValue = MinValueProperty?.GetValue(@object) is { } minVal ? minVal as decimal? ?? 0 : 0;
            MaxValue = MaxValueProperty?.GetValue(@object) is { } maxVal ? maxVal as decimal? ?? 0 : 0;
            EditableMinValue = EditableMinValueProperty?.GetValue(@object) is { } eMinVal ? eMinVal as decimal? ?? 0 : 0;
            EditableMaxValue = EditableMaxValueProperty?.GetValue(@object) is { } eMaxValue ? eMaxValue as decimal? ?? 0 : 0;

            SelectedIndex = SelectedIndexProperty?.GetValue(@object) as int? ?? 0;

            ValueFormat = ValueFormatProperty?.GetValue(@object) as string ?? SettingType switch
            {
                SettingType.Int => "0",
                SettingType.Float => "0.00",
                _ => ""
            };
            CustomFormatter = CustomFormatterProperty?.GetValue(@object) as Type;
            Id = IdProperty?.GetValue(@object) as string ?? "";
        }
    }
}