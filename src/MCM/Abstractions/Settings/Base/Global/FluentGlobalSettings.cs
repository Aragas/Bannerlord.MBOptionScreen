using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.Settings.Models;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MCM.Abstractions.Settings.Base.Global
{
    public class FluentGlobalSettings : GlobalSettings, IFluentSettings
    {
        public static readonly string ContainerId = "MCMv4_Global_FluentStorage";

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
            if (onPropertyChanged != null)
                PropertyChanged += onPropertyChanged;
            Presets = presets;
        }

        public virtual void Register()
        {
            if (AppDomain.CurrentDomain.GetData(ContainerId) == null)
                AppDomain.CurrentDomain.SetData(ContainerId, new Dictionary<string, FluentGlobalSettings>());

            if (AppDomain.CurrentDomain.GetData(ContainerId) is IDictionary dict && !dict.Contains(Id))
                dict.Add(Id, this);
        }
        public virtual void Unregister()
        {
            if (AppDomain.CurrentDomain.GetData(ContainerId) == null)
                AppDomain.CurrentDomain.SetData(ContainerId, new Dictionary<string, FluentGlobalSettings>());

            if (AppDomain.CurrentDomain.GetData(ContainerId) is IDictionary dict && dict.Contains(Id))
                dict.Remove(Id);
        }

        /// <inheritdoc/>
        protected override BaseSettings CreateNew() => null!;
        /// <inheritdoc/>
        protected override BaseSettings CopyAsNew() => null!;
        /// <inheritdoc/>
        public override IDictionary<string, Func<BaseSettings>> GetAvailablePresets()
        {
            // TODO: Presets
            /*
            var dict = new Dictionary<string, Func<BaseSettings>>();
            foreach (var (preset, builder) in Presets)
            {
                Func<BaseSettings> func;
                foreach (var (propertyName, propertyValue) in builder.PropertyValues)
                {
                    
                }
                dict.Add(preset, () =>
                {
                    var settings = CopyAsNew();
                    var props = settings.GetAllSettingPropertyDefinitions();
                    props.FirstOrDefault(x => x.)
                    return settings;
                });
            }
            */

            return new Dictionary<string, Func<BaseSettings>>();
        }
    }
}