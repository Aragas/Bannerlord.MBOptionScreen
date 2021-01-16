using MCM.Abstractions.Ref;

using System;

namespace MCM.Abstractions.Settings.Models.Wrapper
{
    public sealed class SettingsPropertyDefinitionWrapper : ISettingsPropertyDefinition
    {
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

        public SettingsPropertyDefinitionWrapper(object @object) { }
    }
}