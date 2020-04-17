using MBOptionScreen.Interfaces;

using System.Collections.Generic;
using System.Reflection;

namespace MBOptionScreen.Settings
{
    internal class SettingsProviderWrapper : ISettingsProvider
    {
        private readonly object _object;
        private PropertyInfo CreateModSettingsDefinitionsProperty { get; }
        private MethodInfo GetSettingsMethod { get; }
        private MethodInfo OverrideSettingsMethod { get; }
        private MethodInfo RegisterSettingsMethod { get; }
        private MethodInfo ResetSettingsMethod { get; }
        private MethodInfo SaveSettingsMethod { get; }

        public List<ModSettingsDefinition> CreateModSettingsDefinitions => (List < ModSettingsDefinition > ) CreateModSettingsDefinitionsProperty.GetValue(_object);

        public SettingsProviderWrapper(object @object)
        {
            _object = @object;
            var type = @object.GetType();
            CreateModSettingsDefinitionsProperty = type.GetProperty("CreateModSettingsDefinitions", BindingFlags.Instance | BindingFlags.Public);
            GetSettingsMethod = type.GetMethod("GetSettings", BindingFlags.Instance | BindingFlags.Public);
            OverrideSettingsMethod = type.GetMethod("OverrideSettings", BindingFlags.Instance | BindingFlags.Public);
            RegisterSettingsMethod = type.GetMethod("RegisterSettings", BindingFlags.Instance | BindingFlags.Public);
            ResetSettingsMethod = type.GetMethod("ResetSettings", BindingFlags.Instance | BindingFlags.Public);
            SaveSettingsMethod = type.GetMethod("SaveSettings", BindingFlags.Instance | BindingFlags.Public);
        }

        public SettingsBase? GetSettings(string id) => GetSettingsMethod.Invoke(_object, new object[] { id }) as SettingsBase;
        public bool OverrideSettings(SettingsBase settings) => (bool) OverrideSettingsMethod.Invoke(_object, new object[] { settings });
        public bool RegisterSettings(SettingsBase settings) => (bool) RegisterSettingsMethod.Invoke(_object, new object[] { settings });
        public SettingsBase ResetSettings(string id) => ResetSettingsMethod.Invoke(_object, new object[] { id }) as SettingsBase;
        public void SaveSettings(SettingsBase settings) => SaveSettingsMethod.Invoke(_object, new object[] { settings });
    }
}