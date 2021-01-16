using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Base.PerCharacter;
using MCM.Abstractions.Settings.Definitions.Wrapper;
using MCM.Abstractions.Settings.Models;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MCM.Abstractions.FluentBuilder.Implementation
{
    public class DefaultSettingsBuilder : ISettingsBuilder
    {
        private Dictionary<string, ISettingsPropertyGroupBuilder> PropertyGroups { get; } = new();
        
        private string Id { get; }
        private string DisplayName { get; }
        private string FolderName { get; set; } = "";
        private string SubFolder { get; set; } = "";
        private string Format { get; set; } = "memory";
        private int UIVersion { get; set; } = 1;
        private char SubGroupDelimiter { get; set; } = '/';
        private PropertyChangedEventHandler? OnPropertyChanged { get; set; }

        public DefaultSettingsBuilder(string id, string displayName)
        {
            Id = id;
            DisplayName = displayName;

            CreateGroup(SettingsPropertyGroupDefinition.DefaultGroupName, builder => { });
        }

        public ISettingsBuilder SetFolderName(string value) { FolderName = value; return this; }
        public ISettingsBuilder SetSubFolder(string value) { SubFolder = value; return this; }
        public ISettingsBuilder SetFormat(string value) { Format = value; return this; }
        public ISettingsBuilder SetUIVersion(int value) { UIVersion = value; return this; }
        public ISettingsBuilder SetSubGroupDelimiter(char value) { SubGroupDelimiter = value; return this; }
        public ISettingsBuilder SetOnPropertyChanged(PropertyChangedEventHandler value) { OnPropertyChanged = value; return this; }

        public ISettingsBuilder CreateGroup(string name, Action<ISettingsPropertyGroupBuilder> action)
        {
            if (!PropertyGroups.ContainsKey(name))
                PropertyGroups[name] = new DefaultSettingsPropertyGroupBuilder(name);
            action?.Invoke(PropertyGroups[name]);
            return this;
        }

        public FluentGlobalSettings BuildAsGlobal() => new(
            Id, DisplayName, FolderName, SubFolder, Format, UIVersion, SubGroupDelimiter, OnPropertyChanged, GetSettingPropertyGroups());
        public FluentPerCharacterSettings BuildAsPerCharacter() => new(
            Id, DisplayName, FolderName, SubFolder, Format, UIVersion, SubGroupDelimiter, OnPropertyChanged, GetSettingPropertyGroups());

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