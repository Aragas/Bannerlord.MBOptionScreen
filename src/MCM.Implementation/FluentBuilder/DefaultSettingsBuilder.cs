using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Base.PerCampaign;
using MCM.Abstractions.Settings.Definitions.Wrapper;
using MCM.Abstractions.Settings.Models;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MCM.Implementation.FluentBuilder
{
    internal sealed class DefaultSettingsBuilder : BaseSettingsBuilder
    {
        private Dictionary<string, ISettingsPropertyGroupBuilder> PropertyGroups { get; } = new Dictionary<string, ISettingsPropertyGroupBuilder>();
        private Dictionary<string, ISettingsPresetBuilder> Presets { get; } = new Dictionary<string, ISettingsPresetBuilder>();

        private string Id { get; }
        private string DisplayName { get; }
        private string FolderName { get; set; } = string.Empty;
        private string SubFolder { get; set; } = string.Empty;
        private string Format { get; set; } = "memory";
        private int UIVersion { get; set; } = 1;
        private char SubGroupDelimiter { get; set; } = '/';
        private PropertyChangedEventHandler? OnPropertyChanged { get; set; }

        public DefaultSettingsBuilder(string id, string displayName)
        {
            Id = id;
            DisplayName = displayName;

            CreateGroup(SettingsPropertyGroupDefinition.DefaultGroupName, _ => { });
        }

        /// <inheritdoc/>
        public override ISettingsBuilder SetFolderName(string value) { FolderName = value; return this; }
        /// <inheritdoc/>
        public override ISettingsBuilder SetSubFolder(string value) { SubFolder = value; return this; }
        /// <inheritdoc/>
        public override ISettingsBuilder SetFormat(string value) { Format = value; return this; }
        /// <inheritdoc/>
        public override ISettingsBuilder SetUIVersion(int value) { UIVersion = value; return this; }
        /// <inheritdoc/>
        public override ISettingsBuilder SetSubGroupDelimiter(char value) { SubGroupDelimiter = value; return this; }
        /// <inheritdoc/>
        public override ISettingsBuilder SetOnPropertyChanged(PropertyChangedEventHandler value) { OnPropertyChanged = value; return this; }

        /// <inheritdoc/>
        public override ISettingsBuilder CreateGroup(string name, Action<ISettingsPropertyGroupBuilder> builder)
        {
            if (!PropertyGroups.ContainsKey(name))
                PropertyGroups[name] = new DefaultSettingsPropertyGroupBuilder(name);
            builder?.Invoke(PropertyGroups[name]);
            return this;
        }

        /// <inheritdoc/>
        public override ISettingsBuilder CreatePreset(string name, Action<ISettingsPresetBuilder> builder)
        {
            if (!Presets.ContainsKey(name))
                Presets[name] = new DefaultSettingsPresetBuilder(name);
            builder?.Invoke(Presets[name]);
            return this;
        }

        /// <inheritdoc/>
        public override FluentGlobalSettings BuildAsGlobal() => new FluentGlobalSettings(
            Id, DisplayName, FolderName, SubFolder, Format, UIVersion, SubGroupDelimiter, OnPropertyChanged, GetSettingPropertyGroups(), Presets);
        /// <inheritdoc/>
        public override FluentPerCampaignSettings BuildAsPerCampaign() => new FluentPerCampaignSettings(
            Id, DisplayName, FolderName, SubFolder, Format, UIVersion, SubGroupDelimiter, OnPropertyChanged, GetSettingPropertyGroups(), Presets);

        private IEnumerable<SettingsPropertyGroupDefinition> GetSettingPropertyGroups() =>
            SettingsUtils.GetSettingsPropertyGroups(SubGroupDelimiter, GetSettingProperties());

        private IEnumerable<SettingsPropertyDefinition> GetSettingProperties()
        {
            foreach (var settingsPropertyGroup in PropertyGroups.Values)
            foreach (var settingsProperty in settingsPropertyGroup.Properties.Values)
            {
                yield return new SettingsPropertyDefinition(
                    settingsProperty.GetDefinitions(),
                    new PropertyGroupDefinitionWrapper(settingsPropertyGroup.GetPropertyGroupDefinition()),
                    settingsProperty.PropertyReference,
                    SubGroupDelimiter);
            }
        }
    }
}