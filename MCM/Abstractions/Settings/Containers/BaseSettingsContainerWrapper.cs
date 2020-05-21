using HarmonyLib;

using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Models;
using MCM.Abstractions.Settings.Models.Wrapper;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MCM.Abstractions.Settings.Containers
{
    public abstract class BaseSettingsContainerWrapper : ISettingsContainer, IWrapper
    {
        public object Object { get; }
        private PropertyInfo? CreateModSettingsDefinitionsProperty { get; }
        private MethodInfo? GetSettingsMethod { get; }
        private MethodInfo? OverrideSettingsMethod { get; }
        private MethodInfo? ResetSettingsMethod { get; }
        private MethodInfo? SaveSettingsMethod { get; }
        public virtual bool IsCorrect { get; }

        protected BaseSettingsContainerWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();
            CreateModSettingsDefinitionsProperty = AccessTools.Property(type, nameof(CreateModSettingsDefinitions));
            GetSettingsMethod = AccessTools.Method(type, nameof(GetSettings));
            OverrideSettingsMethod = AccessTools.Method(type, nameof(OverrideSettings));
            ResetSettingsMethod = AccessTools.Method(type, nameof(ResetSettings));
            SaveSettingsMethod = AccessTools.Method(type, nameof(SaveSettings));

            IsCorrect = CreateModSettingsDefinitionsProperty != null && GetSettingsMethod != null &&
                        OverrideSettingsMethod != null && ResetSettingsMethod != null && SaveSettingsMethod != null;
        }

        public List<SettingsDefinition> CreateModSettingsDefinitions => 
            ((IEnumerable<object>) (CreateModSettingsDefinitionsProperty?.GetValue(Object) ?? new List<object>()))
            .Select(s => new SettingsDefinitionWrapper(s)).Cast<SettingsDefinition>()
            .ToList();
        public BaseSettings? GetSettings(string id) => GetSettingsMethod?.Invoke(Object, new object[] { id }) is { } settings
                ? settings is BaseSettings settingsBase ? settingsBase : BaseGlobalSettingsWrapper.Create(settings)
                : default;
        public bool OverrideSettings(BaseSettings settings) =>
            OverrideSettingsMethod?.Invoke(Object, new object[] { settings is IWrapper wrapper ? wrapper.Object : settings }) as bool? ?? false;
        public bool ResetSettings(BaseSettings settings) =>
            ResetSettingsMethod?.Invoke(Object, new object[] { settings is IWrapper wrapper ? wrapper.Object : settings }) as bool? ?? false;
        public bool SaveSettings(BaseSettings settings) =>
            SaveSettingsMethod?.Invoke(Object, new object[] { settings is IWrapper wrapper ? wrapper.Object : settings }) as bool? ?? false;
    }
}