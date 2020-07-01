using MCM.Abstractions.Settings.Models;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MCM.Abstractions.Settings.Base.PerCharacter
{
    public class FluentPerCharacterSettings : PerCharacterSettings
    {
        public static readonly string ContainerId = "MCM_PerCharacter_FluentStorage";


        /// <inheritdoc/>
        public sealed override string Id { get; }
        /// <inheritdoc/>
        public sealed override string DisplayName { get; }
        /// <inheritdoc/>
        public sealed override string FolderName { get; }
        /// <inheritdoc/>
        public sealed override string SubFolder { get; }
        /// <inheritdoc/>
        public sealed override string Format { get; }
        /// <inheritdoc/>
        public sealed override int UIVersion { get; }
        /// <inheritdoc/>
        protected sealed override char SubGroupDelimiter { get; }
        /// <inheritdoc/>
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
            if (AppDomain.CurrentDomain.GetData(ContainerId) == null)
                AppDomain.CurrentDomain.SetData(ContainerId, new Dictionary<string, FluentPerCharacterSettings>());

            if (AppDomain.CurrentDomain.GetData(ContainerId) is IDictionary dict && !dict.Contains(Id))
                dict.Add(Id, this);
        }
        public void Unregister()
        {
            if (AppDomain.CurrentDomain.GetData(ContainerId) == null)
                AppDomain.CurrentDomain.SetData(ContainerId, new Dictionary<string, FluentPerCharacterSettings>());

            if (AppDomain.CurrentDomain.GetData(ContainerId) is IDictionary dict && dict.Contains(Id))
                dict.Remove(Id);
        }

        /// <inheritdoc/>
        protected override BaseSettings CreateNew() => null!;
        /// <inheritdoc/>
        protected override BaseSettings CopyAsNew() => null!;
        /// <inheritdoc/>
        public override IDictionary<string, Func<BaseSettings>> GetAvailablePresets() => new Dictionary<string, Func<BaseSettings>>();

        /// <inheritdoc/>
        protected sealed override IEnumerable<SettingsPropertyGroupDefinition> GetUnsortedSettingPropertyGroups() => SettingPropertyGroups;
    }
}