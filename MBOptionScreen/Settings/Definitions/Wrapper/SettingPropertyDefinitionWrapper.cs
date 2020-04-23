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

            if (type.GetProperty("SettingsId")?.GetValue(_object) is string settingsId)
            {
                SettingsId = settingsId;
            }
            // ModLib.GUI.ViewModels.SettingProperty
            else if (type.GetProperty("SettingsInstance") is PropertyInfo settingsInstanceProperty)
            {
                var settingsInstance = settingsInstanceProperty.GetValue(_object);
                var settingsInstanceType = settingsInstance.GetType();
                if (settingsInstanceType.GetProperty("ID")?.GetValue(settingsInstance) is string settingsId1)
                    SettingsId = settingsId1;
            }

            var settingTypeObject = AccessTools.Property(type, "SettingType")?.GetValue(_object);
            SettingType = settingTypeObject != null
                ? Enum.TryParse<SettingType>(settingTypeObject.ToString(), out var resultEnum) ? resultEnum : SettingType.NONE
                : SettingType.NONE;

            Property = new ProxyPropertyInfo((PropertyInfo) type.GetProperty("Property").GetValue(_object));

            switch ((AccessTools.Property(type, "DisplayName") ?? AccessTools.Property(type, "Name")).GetValue(_object))
            {
                case string str:
                    DisplayName = new TextObject(str, null);
                    break;
                case TextObject to:
                    DisplayName = to;
                    break;
            }
            switch (AccessTools.Property(type, "HintText").GetValue(_object))
            {
                case string str:
                    HintText = new TextObject(str, null);
                    break;
                case TextObject to:
                    HintText = to;
                    break;
            }
            Order = type.GetProperty("Order")?.GetValue(_object) as int? ?? -1;
            RequireRestart = type.GetProperty("RequireRestart")?.GetValue(_object) as bool? ?? true;

            GroupName = type.GetProperty("GroupName")?.GetValue(_object) as string ?? "";
            IsMainToggle = type.GetProperty("IsMainToggle")?.GetValue(_object) as bool? ?? false;

            MinValue = type.GetProperty("MinValue")?.GetValue(_object) as float? ?? 0f;
            MaxValue = type.GetProperty("MaxValue")?.GetValue(_object) as float? ?? 0f;
            EditableMinValue = type.GetProperty("EditableMinValue")?.GetValue(_object) as float? ?? 0f;
            EditableMaxValue = type.GetProperty("EditableMaxValue")?.GetValue(_object) as float? ?? 0f;
        }
    }
}