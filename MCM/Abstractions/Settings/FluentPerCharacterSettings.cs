using MCM.Abstractions.Settings.Models;
using MCM.Abstractions.Settings.SettingsContainer;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MCM.Abstractions.Settings
{
    public class FluentPerCharacterSettings : PerCharacterSettings
    {
        public sealed override string Id { get; }
        public sealed override string DisplayName { get; }
        public sealed override string FolderName { get; }
        public sealed override string SubFolder { get; }
        public sealed override string Format { get; }
        public sealed override int UIVersion { get; }
        protected sealed override char SubGroupDelimiter { get; }
        public sealed override event PropertyChangedEventHandler? PropertyChanged { add => base.PropertyChanged += value; remove => base.PropertyChanged -= value; }
        private List<SettingsPropertyGroupDefinition> SettingPropertyGroups { get; }

        public FluentPerCharacterSettings(string id, string displayName, string folderName, string subFolder, string format,
            int uiVersion, char subGroupDelimiter, PropertyChangedEventHandler? onPropertyChanged, IEnumerable<SettingsPropertyGroupDefinition> settingPropertyGroups)
        {
            Id = id;
            DisplayName = displayName;
            FolderName = folderName;
            SubFolder = subFolder;
            Format = format;
            UIVersion = uiVersion;
            SubGroupDelimiter = subGroupDelimiter;
            SettingPropertyGroups = settingPropertyGroups.ToList();
            if (onPropertyChanged != null)
                PropertyChanged += onPropertyChanged;
        }

        public void Register()
        {
            var container = DI.GetImplementation<IFluentPerCharacterSettingsContainer, FluentPerCharacterSettingsContainerWrapper>();
            container!.Register(this);
        }
        public void Unregister()
        {
            var container = DI.GetImplementation<IFluentPerCharacterSettingsContainer, FluentPerCharacterSettingsContainerWrapper>();
            container!.Unregister(this);
        }

        protected override BaseSettings CreateNew() => null;
        protected override BaseSettings CopyAsNew() => null;
        public override IDictionary<string, Func<BaseSettings>> GetAvailablePresets() => new Dictionary<string, Func<BaseSettings>>();

        protected sealed override IEnumerable<SettingsPropertyGroupDefinition> GetUnsortedSettingPropertyGroups() => SettingPropertyGroups;
    }
}