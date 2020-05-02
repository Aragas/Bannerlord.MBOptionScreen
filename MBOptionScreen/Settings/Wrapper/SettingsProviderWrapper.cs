using HarmonyLib;

using System.Collections.Generic;
using System.Reflection;

namespace MBOptionScreen.Settings
{
    internal sealed class SettingsProviderWrapper : IMBOptionScreenSettingsProvider, IModLibSettingsProvider, IWrapper
    {
        internal readonly object _object;
        private PropertyInfo CreateModSettingsDefinitionsProperty { get; }
        private MethodInfo GetSettingsMethod { get; }
        private MethodInfo OverrideSettingsMethod { get; }
        private MethodInfo RegisterSettingsMethod { get; }
        private MethodInfo ResetSettingsMethod { get; }
        private MethodInfo SaveSettingsMethod { get; }
        public bool IsCorrect { get; }

        public List<ModSettingsDefinition> CreateModSettingsDefinitions => (List < ModSettingsDefinition > ) CreateModSettingsDefinitionsProperty.GetValue(_object);

        public SettingsProviderWrapper(object @object)
        {
            _object = @object;
            var type = @object.GetType();
            CreateModSettingsDefinitionsProperty = AccessTools.Property(type, "CreateModSettingsDefinitions");
            GetSettingsMethod = AccessTools.Method(type, "GetSettings");
            OverrideSettingsMethod = AccessTools.Method(type, "OverrideSettings");
            RegisterSettingsMethod = AccessTools.Method(type, "RegisterSettings");
            ResetSettingsMethod = AccessTools.Method(type, "ResetSettings");
            SaveSettingsMethod = AccessTools.Method(type, "SaveSettings");

            IsCorrect = CreateModSettingsDefinitionsProperty != null && GetSettingsMethod != null
                && OverrideSettingsMethod != null && RegisterSettingsMethod != null
                && ResetSettingsMethod != null && SaveSettingsMethod != null;
        }

        public SettingsBase? GetSettings(string id)
        {
            var settings = GetSettingsMethod.Invoke(_object, new object[] {id});
            if (settings is SettingsBase settingsBase)
                return settingsBase;
            if (settings != null)
                return new SettingsWrapper(settings);
            return null;
        }

        public bool OverrideSettings(SettingsBase settings) => (bool) OverrideSettingsMethod.Invoke(_object, new object[] { settings is SettingsWrapper wrapper ? wrapper : settings });
        public bool RegisterSettings(SettingsBase settings) => (bool) RegisterSettingsMethod.Invoke(_object, new object[] { settings is SettingsWrapper wrapper ? wrapper : settings });
        public SettingsBase? ResetSettings(string id)
        {
            var settings = ResetSettingsMethod.Invoke(_object, new object[] { id });
            if (settings is SettingsBase settingsBase)
                return settingsBase;
            return new SettingsWrapper(settings);
        }

        public void SaveSettings(SettingsBase settings) => SaveSettingsMethod.Invoke(_object, new object[] { settings is SettingsWrapper wrapper ? wrapper : settings });
    }
}