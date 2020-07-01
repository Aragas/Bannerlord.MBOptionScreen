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
        private PropertyInfo? MinValueProperty { get; }
        private PropertyInfo? MaxValueProperty { get; }
        private PropertyInfo? EditableMinValueProperty { get; }
        private PropertyInfo? EditableMaxValueProperty { get; }
        private PropertyInfo? SelectedIndexProperty { get; }
        private PropertyInfo? ValueFormatProperty { get; }
        private PropertyInfo? CustomFormatterProperty { get; }
        private PropertyInfo? IdProperty { get; }
        private PropertyInfo? IsToggleProperty { get; }

        /// <inheritdoc/>
        public string SettingsId { get; }
        /// <inheritdoc/>
        public IRef PropertyReference { get; }
        /// <inheritdoc/>
        public SettingType SettingType { get; }
        /// <inheritdoc/>
        public string DisplayName { get; }
        /// <inheritdoc/>
        public int Order { get; }
        /// <inheritdoc/>
        public bool RequireRestart { get; }
        /// <inheritdoc/>
        public string HintText { get; }
        /// <inheritdoc/>
        public decimal MaxValue { get; }
        /// <inheritdoc/>
        public decimal MinValue { get; }
        /// <inheritdoc/>
        public decimal EditableMinValue { get; }
        /// <inheritdoc/>
        public decimal EditableMaxValue { get; }
        /// <inheritdoc/>
        public int SelectedIndex { get; }
        /// <inheritdoc/>
        public string ValueFormat { get; }
        /// <inheritdoc/>
        public Type? CustomFormatter { get; }
        /// <inheritdoc/>
        public string GroupName { get; }
        /// <inheritdoc/>
        [Obsolete("Will be removed", true)]
        public bool IsMainToggle { get; }
        /// <inheritdoc/>
        public int GroupOrder { get; }
        /// <inheritdoc/>
        public string Id { get; }
        /// <inheritdoc/>
        public bool IsToggle { get; }

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
            MinValueProperty = AccessTools.Property(type, nameof(MinValue));
            MaxValueProperty = AccessTools.Property(type, nameof(MaxValue));
            EditableMinValueProperty = AccessTools.Property(type, nameof(EditableMinValue));
            EditableMaxValueProperty = AccessTools.Property(type, nameof(EditableMaxValue));
            SelectedIndexProperty = AccessTools.Property(type, nameof(SelectedIndex));
            ValueFormatProperty = AccessTools.Property(type, nameof(ValueFormat));
            CustomFormatterProperty = AccessTools.Property(type, nameof(CustomFormatter));
            IdProperty = AccessTools.Property(type, nameof(Id));
            IsToggleProperty= AccessTools.Property(type, nameof(IsToggle));


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
                string str => new TextObject(str).ToString(),
                TextObject to => to.ToString(),
                _ => DisplayName
            };
            HintText = HintTextProperty?.GetValue(@object) switch
            {
                string str => new TextObject(str).ToString(),
                TextObject to => to.ToString(),
                _ => HintText
            };
            Order = OrderProperty?.GetValue(@object) as int? ?? -1;
            RequireRestart = RequireRestartProperty?.GetValue(@object) as bool? ?? true;

            GroupName = new TextObject(GroupNameProperty?.GetValue(@object) as string ?? string.Empty).ToString();
            GroupOrder = GroupOrderProperty?.GetValue(@object) as int? ?? -1;

            MinValue = MinValueProperty?.GetValue(@object) is { } minVal ? minVal as decimal? ?? 0 : 0;
            MaxValue = MaxValueProperty?.GetValue(@object) is { } maxVal ? maxVal as decimal? ?? 0 : 0;
            EditableMinValue = EditableMinValueProperty?.GetValue(@object) is { } eMinVal ? eMinVal as decimal? ?? 0 : 0;
            EditableMaxValue = EditableMaxValueProperty?.GetValue(@object) is { } eMaxValue ? eMaxValue as decimal? ?? 0 : 0;

            SelectedIndex = SelectedIndexProperty?.GetValue(@object) as int? ?? 0;

            ValueFormat = ValueFormatProperty?.GetValue(@object) as string ?? SettingType switch
            {
                SettingType.Int => "0",
                SettingType.Float => "0.00",
                _ => string.Empty
            };
            CustomFormatter = CustomFormatterProperty?.GetValue(@object) as Type;
            Id = IdProperty?.GetValue(@object) as string ?? string.Empty;
            IsToggle = IsToggleProperty?.GetValue(@object) as bool? ?? false;
        }
    }
}