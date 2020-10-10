using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.Settings.Models;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MCM.Abstractions.Settings.Base.PerSave
{
    public class FluentPerSaveSettings : PerSaveSettings, IFluentSettings
    {
        public static readonly string ContainerId = "MCM_PerSave_FluentStorage";

        /// <inheritdoc/>
        public sealed override string Id { get; }
        /// <inheritdoc/>
        public sealed override string DisplayName { get; }
        /// <inheritdoc/>
        public sealed override string FolderName { get; }
        /// <inheritdoc/>
        public sealed override string SubFolder { get; }
        /// <inheritdoc/>
        public override string DiscoveryType => "fluent";
        /// <inheritdoc/>
        public sealed override int UIVersion { get; }
        /// <inheritdoc/>
        public sealed override char SubGroupDelimiter { get; }
        /// <inheritdoc/>
        public sealed override event PropertyChangedEventHandler? PropertyChanged { add => base.PropertyChanged += value; remove => base.PropertyChanged -= value; }
        public List<SettingsPropertyGroupDefinition> SettingPropertyGroups { get; }

        public FluentPerSaveSettings(string id, string displayName, string folderName, string subFolder,
            int uiVersion, char subGroupDelimiter, PropertyChangedEventHandler? onPropertyChanged,
            IEnumerable<SettingsPropertyGroupDefinition> settingPropertyGroups, Dictionary<string, ISettingsPresetBuilder> presets)
        {
            Id = id;
            DisplayName = displayName;
            FolderName = folderName;
            SubFolder = subFolder;
            UIVersion = uiVersion;
            SubGroupDelimiter = subGroupDelimiter;
            SettingPropertyGroups = settingPropertyGroups.ToList();
            if (onPropertyChanged != null)
                PropertyChanged += onPropertyChanged;
        }

        public void Register()
        {
            if (AppDomain.CurrentDomain.GetData(ContainerId) == null)
                AppDomain.CurrentDomain.SetData(ContainerId, new Dictionary<string, FluentPerSaveSettings>());

            if (AppDomain.CurrentDomain.GetData(ContainerId) is IDictionary dict && !dict.Contains(Id))
                dict.Add(Id, this);
        }
        public void Unregister()
        {
            if (AppDomain.CurrentDomain.GetData(ContainerId) == null)
                AppDomain.CurrentDomain.SetData(ContainerId, new Dictionary<string, FluentPerSaveSettings>());

            if (AppDomain.CurrentDomain.GetData(ContainerId) is IDictionary dict && dict.Contains(Id))
                dict.Remove(Id);
        }

        /// <inheritdoc/>
        protected override BaseSettings CreateNew() => null!;
        /// <inheritdoc/>
        protected override BaseSettings CopyAsNew() => null!;
        /// <inheritdoc/>
        public override IDictionary<string, Func<BaseSettings>> GetAvailablePresets() => new Dictionary<string, Func<BaseSettings>>();
    }
}