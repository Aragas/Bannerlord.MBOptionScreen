using MCM.Abstractions;
using MCM.Abstractions.Base;
using MCM.Abstractions.Base.Global;
using MCM.Abstractions.Base.PerCampaign;
using MCM.Abstractions.Base.PerSave;
using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.Wrapper;

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MCM.Implementation.FluentBuilder
{
    internal sealed class DefaultSettingsBuilder : BaseSettingsBuilder
    {
        private Dictionary<string, ISettingsPropertyGroupBuilder> PropertyGroups { get; } = new();
        private Dictionary<string, ISettingsPresetBuilder> Presets { get; } = new();

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
            CreatePreset(BaseSettings.DefaultPresetId, BaseSettings.DefaultPresetName, _ => { });
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
            builder.Invoke(PropertyGroups[name]);
            return this;
        }

        /// <inheritdoc/>
        public override ISettingsBuilder CreatePreset(string id, string name, Action<ISettingsPresetBuilder> builder)
        {
            if (!Presets.ContainsKey(id))
                Presets[id] = new DefaultSettingsPresetBuilder(id, name);
            builder.Invoke(Presets[id]);
            return this;
        }

        public override ISettingsBuilder WithoutDefaultPreset()
        {
            Presets.Remove(BaseSettings.DefaultPresetId);
            return this;
        }

        /// <inheritdoc/>
        public override FluentGlobalSettings BuildAsGlobal() => new(
            Id, DisplayName, FolderName, SubFolder, Format, UIVersion, SubGroupDelimiter, OnPropertyChanged, GetSettingPropertyGroups(), Presets.Values);
        /// <inheritdoc/>
        public override FluentPerSaveSettings BuildAsPerSave() => new(
            Id, DisplayName, FolderName, SubFolder, UIVersion, SubGroupDelimiter, OnPropertyChanged, GetSettingPropertyGroups(), Presets.Values);
        /// <inheritdoc/>
        public override FluentPerCampaignSettings BuildAsPerCampaign() => new(
            Id, DisplayName, FolderName, SubFolder, UIVersion, SubGroupDelimiter, OnPropertyChanged, GetSettingPropertyGroups(), Presets.Values);

        private IEnumerable<SettingsPropertyGroupDefinition> GetSettingPropertyGroups() =>
            SettingsUtils.GetSettingsPropertyGroups(SubGroupDelimiter, GetSettingProperties());

        private IEnumerable<SettingsPropertyDefinition> GetSettingProperties()
        {
            foreach (var settingsPropertyGroup in PropertyGroups.Values)
            {
                foreach (var settingsProperty in settingsPropertyGroup.Properties.Values)
                {
                    yield return new SettingsPropertyDefinition(settingsProperty.GetDefinitions(),
                        new PropertyGroupDefinitionWrapper(settingsPropertyGroup.GetPropertyGroupDefinition()),
                        settingsProperty.PropertyReference,
                        SubGroupDelimiter);
                }
            }
        }
    }
}