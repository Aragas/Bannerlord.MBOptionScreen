using Bannerlord.ButterLib.Common.Extensions;

using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.Settings.Models;
using MCM.Extensions;
using MCM.Implementation.FluentBuilder;
using MCM.Implementation.Settings.Containers.Global;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MCM.Abstractions.Settings.Base.Global
{
    public class FluentGlobalSettings : GlobalSettings, IFluentSettings
    {
        /// <inheritdoc/>
        public sealed override string Id { get; }
        /// <inheritdoc/>
        public sealed override string DisplayName { get; }
        /// <inheritdoc/>
        public sealed override string FolderName { get; }
        /// <inheritdoc/>
        public sealed override string SubFolder { get; }
        /// <inheritdoc/>
        public sealed override string FormatType { get; }
        /// <inheritdoc/>
        public override string DiscoveryType => "fluent";
        /// <inheritdoc/>
        public sealed override int UIVersion { get; }
        /// <inheritdoc/>
        public sealed override char SubGroupDelimiter { get; }
        /// <inheritdoc/>
        public sealed override event PropertyChangedEventHandler? PropertyChanged { add => base.PropertyChanged += value; remove => base.PropertyChanged -= value; }
        public List<SettingsPropertyGroupDefinition> SettingPropertyGroups { get; }
        private Dictionary<string, ISettingsPresetBuilder> Presets { get; }

        public FluentGlobalSettings(string id, string displayName, string folderName, string subFolder, string format,
            int uiVersion, char subGroupDelimiter, PropertyChangedEventHandler? onPropertyChanged,
            IEnumerable<SettingsPropertyGroupDefinition> settingPropertyGroups, Dictionary<string, ISettingsPresetBuilder> presets)
        {
            Id = id;
            DisplayName = displayName;
            FolderName = folderName;
            SubFolder = subFolder;
            FormatType = format;
            UIVersion = uiVersion;
            SubGroupDelimiter = subGroupDelimiter;
            SettingPropertyGroups = settingPropertyGroups.ToList();
            if (onPropertyChanged is not null)
                PropertyChanged += onPropertyChanged;

            Presets = new Dictionary<string, ISettingsPresetBuilder>();
            var defaultKey = base.GetAvailablePresets().First().Key;
            if (!presets.ContainsKey(defaultKey))
            {
                var defaultPreset = new DefaultSettingsPresetBuilder(defaultKey);
                foreach (var propertyDefinition in this.GetAllSettingPropertyDefinitions())
                    defaultPreset.SetPropertyValue(propertyDefinition.Id, propertyDefinition.PropertyReference.Value);
                Presets.Add(defaultPreset.PresetName, defaultPreset);
            }
            foreach (var (key, value) in presets)
                Presets.Add(key, value);
        }

        public void Register()
        {
            var container = MCMSubModule.Instance?.GetServiceProvider()?.GetRequiredService<IMCMFluentGlobalSettingsContainer>();
            container?.Register(this);
        }
        public void Unregister()
        {
            var container = MCMSubModule.Instance?.GetServiceProvider()?.GetRequiredService<IMCMFluentGlobalSettingsContainer>();
            container?.Unregister(this);
        }

        /// <inheritdoc/>
        protected sealed override BaseSettings CreateNew() => new FluentGlobalSettings(
            Id,
            DisplayName,
            FolderName,
            SubFolder,
            FormatType,
            UIVersion,
            SubGroupDelimiter,
            null,
            SettingPropertyGroups.Select(g => g.Clone(false)),
            Presets);

        /// <inheritdoc/>
        public sealed override IDictionary<string, Func<BaseSettings>> GetAvailablePresets()
        {
            var dict = new Dictionary<string, Func<BaseSettings>>();
            foreach (var (presetName, presetBuilder) in Presets)
            {
                dict.Add(presetName, () =>
                {
                    var settings = CreateNew();
                    var props = settings.GetAllSettingPropertyDefinitions().ToList();
                    foreach (var (propertyId, propertyValue) in presetBuilder.PropertyValues)
                    {
                        if (props.Find(x => x.Id == propertyId) is { } property)
                            property.PropertyReference.Value = propertyValue;
                    }
                    return settings;
                });
            }
            return dict;
        }
    }
}