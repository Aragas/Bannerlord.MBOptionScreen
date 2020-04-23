using HarmonyLib;
using MBOptionScreen.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MBOptionScreen.Settings
{
    internal class SettingsWrapper : SettingsBase
    {
        internal readonly object _object;

        private PropertyInfo IdProperty { get; }
        private PropertyInfo ModuleFolderNameProperty { get; }
        private PropertyInfo ModNameProperty { get; }
        private PropertyInfo UIVersionProperty { get; }
        private PropertyInfo SubFolderProperty { get; }
        private PropertyInfo SubGroupDelimiterProperty { get; }
        private PropertyInfo FormatProperty { get; }
        private MethodInfo GetSettingPropertyGroupsMethod { get; }

        public override string Id { get => (string)IdProperty.GetValue(_object); set => IdProperty.SetValue(_object, value); }
        public override string ModuleFolderName => (string)ModuleFolderNameProperty.GetValue(_object);
        public override string ModName => (string)ModNameProperty.GetValue(_object);
        public override int UIVersion => UIVersionProperty?.GetValue(_object) as int? ?? 1;
        public override string SubFolder => SubFolderProperty?.GetValue(_object) as string ?? "";
        protected override char SubGroupDelimiter => SubGroupDelimiterProperty?.GetValue(_object) as char? ?? '/';
        public override string Format => FormatProperty?.GetValue(_object) as string ?? "json";

        public SettingsWrapper() : this(new StubSettings()) { }
        public SettingsWrapper(object @object)
        {
            _object = @object;

            var type = @object.GetType();
            IdProperty = AccessTools.Property(type, "Id") ?? AccessTools.Property(type, "ID");
            ModuleFolderNameProperty = AccessTools.Property(type, "ModuleFolderName");
            ModNameProperty = AccessTools.Property(type, "ModName");
            UIVersionProperty = AccessTools.Property(type, "UIVersion");
            SubFolderProperty = AccessTools.Property(type, "SubFolder");
            SubGroupDelimiterProperty = AccessTools.Property(type, "SubGroupDelimiter");
            FormatProperty = AccessTools.Property(type, "Format");
            GetSettingPropertyGroupsMethod = AccessTools.Method(type, "GetSettingPropertyGroups");
        }

        public override List<SettingPropertyGroupDefinition> GetSettingPropertyGroups() => GetWrappedSettingPropertyGroups()
                .OrderByDescending(x => x.GroupName == SettingPropertyGroupDefinition.DefaultGroupName)
                .ThenByDescending(x => x.Order)
                .ThenByDescending(x => x, new AlphanumComparatorFast())
                .ToList();

        private IEnumerable<SettingPropertyGroupDefinition> GetWrappedSettingPropertyGroups()
        {
            var list = ((IList) GetSettingPropertyGroupsMethod.Invoke(_object, Array.Empty<object>()));
            foreach (var @object in list)
            {
                yield return new SettingPropertyGroupDefinitionWrapper(@object);
            }
        }
    }
}