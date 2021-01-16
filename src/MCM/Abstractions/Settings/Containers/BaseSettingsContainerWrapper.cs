using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Models;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings.Containers
{
    public abstract class BaseSettingsContainerWrapper : ISettingsContainer, IWrapper
    {
        public object Object { get; }
        public virtual bool IsCorrect { get; }

        protected BaseSettingsContainerWrapper(object @object) { }

        public List<SettingsDefinition> CreateModSettingsDefinitions => new();
        public BaseSettings? GetSettings(string id) => null;
        public bool OverrideSettings(BaseSettings settings) => false;
        public bool ResetSettings(BaseSettings settings) => false;
        public bool SaveSettings(BaseSettings settings) => false;
    }
}