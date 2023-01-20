using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.Utils;
using MCM.Abstractions.Xml;
using MCM.Common;

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace MCM.Abstractions.Base.Global
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    sealed class ExternalGlobalSettings : FluentGlobalSettings
    {
        private static SettingsPropertyDefinition FromXml(IPropertyGroupDefinition group, PropertyBaseXmlModel xmlModel, char subGroupDelimiter) => xmlModel switch
        {
            PropertyBoolXmlModel model =>
                new SettingsPropertyDefinition(SettingsUtils.GetPropertyDefinitionWrappers(model), group, new StorageRef<bool>(model.Value), subGroupDelimiter),
            PropertyDropdownXmlModel model =>
                new SettingsPropertyDefinition(SettingsUtils.GetPropertyDefinitionWrappers(model), group, new StorageRef<Dropdown<string>>(new Dropdown<string>(model.Values, model.SelectedIndex)), subGroupDelimiter),
            PropertyFloatingIntegerXmlModel model =>
                new SettingsPropertyDefinition(SettingsUtils.GetPropertyDefinitionWrappers(model), group, new StorageRef<float>((float) model.Value), subGroupDelimiter),
            PropertyIntegerXmlModel model =>
                new SettingsPropertyDefinition(SettingsUtils.GetPropertyDefinitionWrappers(model), group, new StorageRef<int>((int) model.Value), subGroupDelimiter),
            PropertyTextXmlModel model =>
                new SettingsPropertyDefinition(SettingsUtils.GetPropertyDefinitionWrappers(model), group, new StorageRef<string>(model.Value), subGroupDelimiter),
        };

        public static ExternalGlobalSettings? CreateFromXmlFile(string filePath, PropertyChangedEventHandler? propertyChanged = null)
        {
            using var xmlStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            if (SerializationUtils.DeserializeXml<SettingsXmlModel>(xmlStream) is not { } settingsXmlModel)
                return null;

            var subGroupDelimiter = settingsXmlModel.SubGroupDelimiter[0];

            var props = settingsXmlModel.Groups
                .SelectMany(g => g.Properties.Select(p => FromXml(g, p, subGroupDelimiter)))
                .Concat(settingsXmlModel.Properties.Select(p => FromXml(SettingsPropertyGroupDefinition.DefaultGroup, p, subGroupDelimiter)));
            var propGroups = SettingsUtils.GetSettingsPropertyGroups(subGroupDelimiter, props);

            return new ExternalGlobalSettings(
                settingsXmlModel.Id,
                settingsXmlModel.DisplayName,
                settingsXmlModel.FolderName,
                settingsXmlModel.SubFolder,
                settingsXmlModel.FormatType,
                settingsXmlModel.UIVersion,
                subGroupDelimiter,
                propertyChanged,
                propGroups,
                new List<ISettingsPresetBuilder>(), filePath);
        }

        public string FilePath { get; init; }

        private ExternalGlobalSettings(string id, string displayName, string folderName, string subFolder, string format, int uiVersion, char subGroupDelimiter, PropertyChangedEventHandler? onPropertyChanged, IEnumerable<SettingsPropertyGroupDefinition> settingPropertyGroups, IEnumerable<ISettingsPresetBuilder> presets, string filePath)
            : base(id, displayName, folderName, subFolder, format, uiVersion, subGroupDelimiter, onPropertyChanged, settingPropertyGroups, presets)
        {
            FilePath = filePath;
        }
    }
}