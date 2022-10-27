using BUTR.DependencyInjection;

using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.Global;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MCM.Abstractions.Base.Global
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
        private List<ISettingsPresetBuilder> Presets { get; }

        public FluentGlobalSettings(string id, string displayName, string folderName, string subFolder, string format,
            int uiVersion, char subGroupDelimiter, PropertyChangedEventHandler? onPropertyChanged,
            IEnumerable<SettingsPropertyGroupDefinition> settingPropertyGroups, IEnumerable<ISettingsPresetBuilder> presets)
        {
            Id = id;
            DisplayName = displayName;
            FolderName = folderName;
            SubFolder = subFolder;
            FormatType = format;
            UIVersion = uiVersion;
            SubGroupDelimiter = subGroupDelimiter;
            SettingPropertyGroups = settingPropertyGroups.ToList();
            Presets = presets.ToList();
            if (onPropertyChanged is not null)
                PropertyChanged += onPropertyChanged;
        }

        public void Register()
        {
            var containers = GenericServiceProvider.GetService<IEnumerable<IFluentGlobalSettingsContainer>>() ?? Enumerable.Empty<IFluentGlobalSettingsContainer>();
            foreach (var container in containers)
            {
                container?.Register(this);
            }
        }
        public void Unregister()
        {
            var containers = GenericServiceProvider.GetService<IEnumerable<IFluentGlobalSettingsContainer>>() ?? Enumerable.Empty<IFluentGlobalSettingsContainer>();
            foreach (var container in containers)
            {
                container?.Unregister(this);
            }
        }

        /// <inheritdoc/>
        public sealed override BaseSettings CreateNew() => new FluentGlobalSettings(
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
        public sealed override IEnumerable<ISettingsPreset> GetBuiltInPresets() => Presets.Select(presetBuilder => presetBuilder.Build(CreateNew()));
    }
}