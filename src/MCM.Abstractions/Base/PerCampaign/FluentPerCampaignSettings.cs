using BUTR.DependencyInjection;

using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.PerCampaign;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MCM.Abstractions.Base.PerCampaign
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    class FluentPerCampaignSettings : PerCampaignSettings, IFluentSettings
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
        public override string DiscoveryType => "fluent";
        /// <inheritdoc/>
        public sealed override int UIVersion { get; }
        /// <inheritdoc/>
        public sealed override char SubGroupDelimiter { get; }
        /// <inheritdoc/>
        public sealed override event PropertyChangedEventHandler? PropertyChanged { add => base.PropertyChanged += value; remove => base.PropertyChanged -= value; }
        public List<SettingsPropertyGroupDefinition> SettingPropertyGroups { get; }
        private List<ISettingsPresetBuilder> Presets { get; }

        public FluentPerCampaignSettings(string id, string displayName, string folderName, string subFolder, int uiVersion, char subGroupDelimiter, PropertyChangedEventHandler? onPropertyChanged,
            IEnumerable<SettingsPropertyGroupDefinition> settingPropertyGroups, IEnumerable<ISettingsPresetBuilder> presets)
        {
            Id = id;
            DisplayName = displayName;
            FolderName = folderName;
            SubFolder = subFolder;
            UIVersion = uiVersion;
            SubGroupDelimiter = subGroupDelimiter;
            SettingPropertyGroups = settingPropertyGroups.ToList();
            Presets = presets.ToList();
            if (onPropertyChanged is not null)
                PropertyChanged += onPropertyChanged;
        }

        public void Register()
        {
            // TODO: check
            var containers = GenericServiceProvider.GetService<IEnumerable<IFluentPerCampaignSettingsContainer>>() ?? Enumerable.Empty<IFluentPerCampaignSettingsContainer>();
            foreach (var container in containers)
            {
                container?.Register(this);
            }
        }
        public void Unregister()
        {
            // TODO: check
            var containers = GenericServiceProvider.GetService<IEnumerable<IFluentPerCampaignSettingsContainer>>() ?? Enumerable.Empty<IFluentPerCampaignSettingsContainer>();
            foreach (var container in containers)
            {
                container?.Unregister(this);
            }
        }

        /// <inheritdoc/>
        public override BaseSettings CreateNew() => new FluentPerCampaignSettings(
            Id,
            DisplayName,
            FolderName,
            SubFolder,
            UIVersion,
            SubGroupDelimiter,
            null,
            SettingPropertyGroups.Select(g => g.Clone(false)),
            Presets);

        /// <inheritdoc/>
        public sealed override IEnumerable<ISettingsPreset> GetBuiltInPresets() => Presets.Select(presetBuilder => presetBuilder.Build(CreateNew()));
    }
}