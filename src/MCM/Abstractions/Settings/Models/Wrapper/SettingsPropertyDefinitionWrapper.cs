using HarmonyLib;

using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Definitions.Wrapper;
using MCM.Utils;

using System;

using TaleWorlds.Localization;

namespace MCM.Abstractions.Settings.Models.Wrapper
{
    public sealed class SettingsPropertyDefinitionWrapper : ISettingsPropertyDefinition
    {
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
        public int GroupOrder { get; }
        /// <inheritdoc/>
        public string Id { get; }
        /// <inheritdoc/>
        public bool IsToggle { get; }
        private char SubGroupDelimiter { get; }

        public SettingsPropertyDefinitionWrapper(object @object)
        {
            var type = @object.GetType();

            var settingsIdProperty = AccessTools.Property(type, nameof(SettingsId));
            var settingTypeProperty = AccessTools.Property(type, nameof(SettingType));
            var propertyProperty = AccessTools.Property(type, nameof(PropertyReference));
            var displayNameProperty = AccessTools.Property(type, nameof(DisplayName));
            var hintTextProperty = AccessTools.Property(type, nameof(HintText));
            var orderProperty = AccessTools.Property(type, nameof(Order));
            var requireRestartProperty = AccessTools.Property(type, nameof(RequireRestart));
            var groupNameProperty = AccessTools.Property(type, nameof(GroupName));
            var groupOrderProperty = AccessTools.Property(type, nameof(GroupOrder));
            var minValueProperty = AccessTools.Property(type, nameof(MinValue));
            var maxValueProperty = AccessTools.Property(type, nameof(MaxValue));
            var editableMinValueProperty = AccessTools.Property(type, nameof(EditableMinValue));
            var editableMaxValueProperty = AccessTools.Property(type, nameof(EditableMaxValue));
            var selectedIndexProperty = AccessTools.Property(type, nameof(SelectedIndex));
            var valueFormatProperty = AccessTools.Property(type, nameof(ValueFormat));
            var customFormatterProperty = AccessTools.Property(type, nameof(CustomFormatter));
            var idProperty = AccessTools.Property(type, nameof(Id));
            var isToggleProperty= AccessTools.Property(type, nameof(IsToggle));
            var subGroupDelimiterProperty= AccessTools.Property(type, nameof(SubGroupDelimiter));


            SettingsId = settingsIdProperty?.GetValue(@object) as string ?? "ERROR";
            SettingType = settingTypeProperty?.GetValue(@object) is { } settingTypeObject
                ? Enum.TryParse<SettingType>(settingTypeObject.ToString(), out var resultEnum)
                    ? resultEnum
                    : SettingType.NONE
                : SettingType.NONE;
            PropertyReference = propertyProperty?.GetValue(@object) is { } value
                ? value is IRef @ref ? @ref : new RefWrapper(value)
                : new ProxyRef<object?>(() => null, _ => { });

            DisplayName = displayNameProperty?.GetValue(@object) switch
            {
                string str => new TextObject(str).ToString(),
                TextObject to => to.ToString(),
                _ => DisplayName
            };
            HintText = hintTextProperty?.GetValue(@object) switch
            {
                string str => new TextObject(str).ToString(),
                TextObject to => to.ToString(),
                _ => HintText
            };
            Order = orderProperty?.GetValue(@object) as int? ?? -1;
            RequireRestart = requireRestartProperty?.GetValue(@object) as bool? ?? true;

            GroupName = new TextObject(groupNameProperty?.GetValue(@object) as string ?? string.Empty).ToString();
            GroupOrder = groupOrderProperty?.GetValue(@object) as int? ?? -1;

            MinValue = minValueProperty?.GetValue(@object) is { } minVal ? minVal as decimal? ?? 0 : 0;
            MaxValue = maxValueProperty?.GetValue(@object) is { } maxVal ? maxVal as decimal? ?? 0 : 0;
            EditableMinValue = editableMinValueProperty?.GetValue(@object) is { } eMinVal ? eMinVal as decimal? ?? 0 : 0;
            EditableMaxValue = editableMaxValueProperty?.GetValue(@object) is { } eMaxValue ? eMaxValue as decimal? ?? 0 : 0;

            SelectedIndex = selectedIndexProperty?.GetValue(@object) as int? ?? 0;

            ValueFormat = valueFormatProperty?.GetValue(@object) as string ?? SettingType switch
            {
                SettingType.Int => "0",
                SettingType.Float => "0.00",
                _ => string.Empty
            };
            CustomFormatter = customFormatterProperty?.GetValue(@object) as Type;
            Id = idProperty?.GetValue(@object) as string ?? string.Empty;
            IsToggle = isToggleProperty?.GetValue(@object) as bool? ?? false;

            SubGroupDelimiter = subGroupDelimiterProperty?.GetValue(@object) as char? ?? '/';
        }

        public SettingsPropertyDefinition Clone(bool keepRefs = true)
        {
            var localPropValue = PropertyReference.Value;
            return new SettingsPropertyDefinition(
                SettingsUtils.GetPropertyDefinitionWrappers(this),
                new PropertyGroupDefinitionWrapper(this),
                keepRefs ? PropertyReference : new StorageRef(localPropValue),
                SubGroupDelimiter);
        }
    }
}