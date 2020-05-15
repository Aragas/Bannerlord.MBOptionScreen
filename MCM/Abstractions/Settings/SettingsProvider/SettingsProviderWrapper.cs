using HarmonyLib;

using MCM.Abstractions.Settings.Models;
using MCM.Abstractions.Settings.Models.Wrapper;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TaleWorlds.Core;

namespace MCM.Abstractions.Settings.SettingsProvider
{
    /// <summary>
    /// For DI
    /// </summary>
    public sealed class SettingsProviderWrapper : BaseSettingsProvider, IWrapper
    {
        public object Object { get; }
        private PropertyInfo? CreateModSettingsDefinitionsProperty { get; }
        private MethodInfo? GetSettingsMethod { get; }
        private MethodInfo? SaveSettingsMethod { get; }
        private MethodInfo? ResetSettingsMethod { get; }
        private MethodInfo? OverrideSettingsMethod { get; }
        private MethodInfo? OnGameStartedMethod { get; }
        private MethodInfo? OnGameEndedMethod { get; }
        public bool IsCorrect { get; }

        public SettingsProviderWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();

            CreateModSettingsDefinitionsProperty = AccessTools.Property(type, nameof(CreateModSettingsDefinitions));
            GetSettingsMethod = AccessTools.Method(type, nameof(GetSettings));
            SaveSettingsMethod = AccessTools.Method(type, nameof(SaveSettings));
            ResetSettingsMethod = AccessTools.Method(type, nameof(ResetSettings));
            OverrideSettingsMethod = AccessTools.Method(type, nameof(OverrideSettings));
            OnGameStartedMethod = AccessTools.Method(type, nameof(OnGameStarted));
            OnGameEndedMethod = AccessTools.Method(type, nameof(OnGameEnded));

            IsCorrect = CreateModSettingsDefinitionsProperty != null &&
                        GetSettingsMethod != null && SaveSettingsMethod != null &&
                        ResetSettingsMethod != null && OverrideSettingsMethod != null &&
                        OnGameStartedMethod != null && OnGameEndedMethod != null;
        }

        public override IEnumerable<SettingsDefinition> CreateModSettingsDefinitions =>
            ((IEnumerable<object>) (CreateModSettingsDefinitionsProperty?.GetValue(Object) ?? new List<object>()))
            .Select(s => new SettingsDefinitionWrapper(s));
        public override BaseSettings? GetSettings(string id) => 
            GetSettingsMethod?.Invoke(Object, new object[] { id }) as BaseSettings;
        public override void SaveSettings(BaseSettings settings) =>
            SaveSettingsMethod?.Invoke(Object, new object[] { settings });
        public override void ResetSettings(BaseSettings settings) =>
            ResetSettingsMethod?.Invoke(Object, new object[] { settings });
        public override void OverrideSettings(BaseSettings settings) =>
            OverrideSettingsMethod?.Invoke(Object, new object[] { settings });
        public override void OnGameStarted(Game game) =>
            OnGameStartedMethod?.Invoke(Object, new object[] { game });
        public override void OnGameEnded(Game game) =>
            OnGameEndedMethod?.Invoke(Object, new object[] { game });
    }
}