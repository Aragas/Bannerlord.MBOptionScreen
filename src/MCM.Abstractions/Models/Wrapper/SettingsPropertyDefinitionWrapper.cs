using HarmonyLib.BUTR.Extensions;

using MCM.Common;

using System;

namespace MCM.Abstractions.Wrapper
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    sealed class SettingsPropertyDefinitionWrapper : ISettingsPropertyDefinition
    {
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
        /// <inheritdoc/>
        public string Content { get; }

        public SettingsPropertyDefinitionWrapper(object @object)
        {
            var type = @object.GetType();

            var settingTypeProperty = AccessTools2.Property(type, nameof(SettingType));
            var propertyProperty = AccessTools2.Property(type, nameof(PropertyReference));
            var displayNameProperty = AccessTools2.Property(type, nameof(DisplayName));
            var hintTextProperty = AccessTools2.Property(type, nameof(HintText));
            var orderProperty = AccessTools2.Property(type, nameof(Order));
            var requireRestartProperty = AccessTools2.Property(type, nameof(RequireRestart));
            var groupNameProperty = AccessTools2.Property(type, nameof(GroupName));
            var groupOrderProperty = AccessTools2.Property(type, nameof(GroupOrder));
            var minValueProperty = AccessTools2.Property(type, nameof(MinValue));
            var maxValueProperty = AccessTools2.Property(type, nameof(MaxValue));
            var editableMinValueProperty = AccessTools2.Property(type, nameof(EditableMinValue));
            var editableMaxValueProperty = AccessTools2.Property(type, nameof(EditableMaxValue));
            var selectedIndexProperty = AccessTools2.Property(type, nameof(SelectedIndex));
            var valueFormatProperty = AccessTools2.Property(type, nameof(ValueFormat));
            var customFormatterProperty = AccessTools2.Property(type, nameof(CustomFormatter));
            var idProperty = AccessTools2.Property(type, nameof(Id));
            var isToggleProperty = AccessTools2.Property(type, nameof(IsToggle));
            var subGroupDelimiterProperty = AccessTools2.Property(type, nameof(SubGroupDelimiter));
            var contentProperty = AccessTools2.Property(type, nameof(Content));


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
                string str => str,
                { } to => to.ToString(),
                _ => "ERROR"
            };
            HintText = hintTextProperty?.GetValue(@object) switch
            {
                string str => str,
                { } to => to.ToString(),
                _ => "ERROR"
            };
            Order = orderProperty?.GetValue(@object) as int? ?? -1;
            RequireRestart = requireRestartProperty?.GetValue(@object) as bool? ?? true;

            GroupName = groupNameProperty?.GetValue(@object) as string ?? string.Empty;
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
            Content = contentProperty?.GetValue(@object) as string ?? string.Empty;
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