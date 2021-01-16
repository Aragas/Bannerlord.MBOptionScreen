using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Formats;
using MCM.Abstractions.Settings.Models;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings.Containers
{
    public abstract class BaseSettingsContainer<TSettings> : ISettingsContainer where TSettings : BaseSettings
    {
        protected virtual string RootFolder { get; }
        protected Dictionary<string, ISettingsFormat> AvailableSettingsFormats { get; } = new();
        protected virtual Dictionary<string, TSettings> LoadedSettings { get; } = new();

        public virtual List<SettingsDefinition> CreateModSettingsDefinitions => new();

        protected BaseSettingsContainer() { }

        protected virtual void RegisterSettings(TSettings tSettings) { }

        public virtual BaseSettings? GetSettings(string id) => null;
        public virtual bool SaveSettings(BaseSettings settings) => false;

        public virtual bool OverrideSettings(BaseSettings settings) => false;
        public virtual bool ResetSettings(BaseSettings settings) => false;
    }
}