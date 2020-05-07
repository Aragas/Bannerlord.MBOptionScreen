using HarmonyLib;

using MCM.Utils;

using System;
using System.Reflection;

using TaleWorlds.Localization;

namespace MCM.Abstractions.Settings.Definitions.Wrapper
{
    public sealed class SettingsPropertyDefinitionWrapper : SettingsPropertyDefinition
    {
        private readonly object _object;

        public SettingsPropertyDefinitionWrapper(object @object) : base()
        {
            _object = @object;
            var type = @object.GetType();

            switch (type.FullName)
            {
                case "ModLib.GUI.ViewModels.SettingProperty":
                    ParseModLib(@object);
                    break;
                default:
                    Parse(@object);
                    break;
            }
        }

        private void Parse(object @object)
        {
            var type = @object.GetType();

            SettingsId = AccessTools.Property(type, nameof(SettingsId))?.GetValue(@object) as string ?? "ERROR";
            var settingTypeObject = AccessTools.Property(type, nameof(SettingType))?.GetValue(@object);
            SettingType = settingTypeObject != null
                ? Enum.TryParse<SettingType>(settingTypeObject.ToString(), out var resultEnum) ? resultEnum : SettingType.NONE
                : SettingType.NONE;
            Property = new WrapperPropertyInfo((PropertyInfo) type.GetProperty(nameof(Property)).GetValue(@object));

            DisplayName = (AccessTools.Property(type, nameof(DisplayName)) ?? AccessTools.Property(type, "Name"))?.GetValue(@object) switch
            {
                string str => new TextObject(str, null),
                TextObject to => to,
                _ => DisplayName
            };
            HintText = AccessTools.Property(type, nameof(HintText))?.GetValue(@object) switch
            {
                string str => new TextObject(str, null),
                TextObject to => to,
                _ => HintText
            };
            Order = type.GetProperty(nameof(Order))?.GetValue(@object) as int? ?? -1;
            RequireRestart = type.GetProperty(nameof(RequireRestart))?.GetValue(@object) as bool? ?? true;

            GroupName = type.GetProperty(nameof(GroupName))?.GetValue(@object) as string ?? "";
            IsMainToggle = type.GetProperty(nameof(IsMainToggle))?.GetValue(@object) as bool? ?? false;

            MinValue = type.GetProperty(nameof(MinValue))?.GetValue(@object) as float? ?? 0f;
            MaxValue = type.GetProperty(nameof(MaxValue))?.GetValue(@object) as float? ?? 0f;
            EditableMinValue = type.GetProperty(nameof(EditableMinValue))?.GetValue(@object) as float? ?? 0f;
            EditableMaxValue = type.GetProperty(nameof(EditableMaxValue))?.GetValue(@object) as float? ?? 0f;

            SelectedIndex = type.GetProperty(nameof(SelectedIndex))?.GetValue(@object) as int? ?? 0;
        }

        private void ParseModLib(object @object)
        {
            var type = @object.GetType();

            var settingAttributeProperty = AccessTools.Property(type, "SettingAttribute");
            var settingAttribute = settingAttributeProperty.GetValue(@object);
            var settingAttributeType = settingAttribute.GetType();

            var groupAttributeProperty = AccessTools.Property(type, "GroupAttribute");
            var groupAttribute = groupAttributeProperty.GetValue(@object);
            var groupAttributeType = groupAttribute.GetType();

            Order = -1;
            RequireRestart = true;

            GroupName = AccessTools.Property(groupAttributeType, "GroupName")?.GetValue(groupAttribute) as string ?? "";
            IsMainToggle = AccessTools.Property(groupAttributeType, "IsMainToggle")?.GetValue(groupAttribute) as bool? ?? false;


            var settingsInstance = AccessTools.Property(type, "SettingsInstance")?.GetValue(@object);
            SettingsId = AccessTools.Property(settingsInstance!.GetType(), "ID")?.GetValue(settingsInstance) as string ?? "ERROR";
            Property = new WrapperPropertyInfo((PropertyInfo) AccessTools.Property(type, "Property").GetValue(@object));
            var settingTypeObject = AccessTools.Property(type, "SettingType")?.GetValue(@object);
            SettingType = settingTypeObject != null
                ? Enum.TryParse<SettingType>(settingTypeObject.ToString(), out var resultEnum) ? resultEnum : SettingType.NONE
                : SettingType.NONE;

            DisplayName = new TextObject(settingAttributeType.GetProperty("DisplayName")?.GetValue(settingAttribute) as string ?? "", null);
            MinValue = AccessTools.Property(settingAttributeType, "MinValue")?.GetValue(settingAttribute) as float? ?? 0f;
            MaxValue = AccessTools.Property(settingAttributeType, "MaxValue")?.GetValue(settingAttribute) as float? ?? 0f;
            EditableMinValue = AccessTools.Property(settingAttributeType, "EditableMinValue")?.GetValue(settingAttribute) as float? ?? 0f;
            EditableMaxValue = AccessTools.Property(settingAttributeType, "EditableMaxValue")?.GetValue(settingAttribute) as float? ?? 0f;
            HintText = new TextObject(AccessTools.Property(settingAttributeType, "HintText")?.GetValue(settingAttribute) as string ?? "", null);
        }
    }
}