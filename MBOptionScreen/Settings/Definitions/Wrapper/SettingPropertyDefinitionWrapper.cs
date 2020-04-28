using HarmonyLib;

using MBOptionScreen.Utils;

using System;
using System.Reflection;

using TaleWorlds.Localization;

namespace MBOptionScreen.Settings
{
    internal sealed class SettingPropertyDefinitionWrapper : SettingPropertyDefinition
    {
        private readonly object _object;

        public SettingPropertyDefinitionWrapper(object @object) : base()
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

            SettingsId = AccessTools.Property(type, "SettingsId")?.GetValue(@object) as string ?? "ERROR";
            var settingTypeObject = AccessTools.Property(type, "SettingType")?.GetValue(@object);
            SettingType = settingTypeObject != null
                ? Enum.TryParse<SettingType>(settingTypeObject.ToString(), out var resultEnum) ? resultEnum : SettingType.NONE
                : SettingType.NONE;
            Property = new ProxyPropertyInfo((PropertyInfo)type.GetProperty("Property").GetValue(@object));

            switch ((AccessTools.Property(type, "DisplayName") ?? AccessTools.Property(type, "Name")).GetValue(@object))
            {
                case string str:
                    DisplayName = new TextObject(str, null);
                    break;
                case TextObject to:
                    DisplayName = to;
                    break;
            }
            switch (AccessTools.Property(type, "HintText").GetValue(@object))
            {
                case string str:
                    HintText = new TextObject(str, null);
                    break;
                case TextObject to:
                    HintText = to;
                    break;
            }
            Order = type.GetProperty("Order")?.GetValue(@object) as int? ?? -1;
            RequireRestart = type.GetProperty("RequireRestart")?.GetValue(@object) as bool? ?? true;

            GroupName = type.GetProperty("GroupName")?.GetValue(@object) as string ?? "";
            IsMainToggle = type.GetProperty("IsMainToggle")?.GetValue(@object) as bool? ?? false;

            MinValue = type.GetProperty("MinValue")?.GetValue(@object) as float? ?? 0f;
            MaxValue = type.GetProperty("MaxValue")?.GetValue(@object) as float? ?? 0f;
            EditableMinValue = type.GetProperty("EditableMinValue")?.GetValue(@object) as float? ?? 0f;
            EditableMaxValue = type.GetProperty("EditableMaxValue")?.GetValue(@object) as float? ?? 0f;
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
            Property = new ProxyPropertyInfo((PropertyInfo)AccessTools.Property(type, "Property").GetValue(@object));
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