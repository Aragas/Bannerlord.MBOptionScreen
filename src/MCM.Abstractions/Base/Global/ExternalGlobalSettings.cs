using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.Xml;
using MCM.Common;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace MCM.Abstractions.Base.Global
{
    public sealed class ExternalGlobalSettings : FluentGlobalSettings
    {
        private static SettingsPropertyDefinition FromXml(IPropertyGroupDefinition group, PropertyBaseXmlModel xmlModel, char subGroupDelimiter)
        {
            return xmlModel switch
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
                _ => throw new Exception()
            };
        }

        public static ExternalGlobalSettings? CreateFromXmlFile(string filePath, PropertyChangedEventHandler? propertyChanged = null)
        {
            using var xmlStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = XmlReader.Create(xmlStream, new XmlReaderSettings { IgnoreComments = true, IgnoreWhitespace = true });
            var serializer = new XmlSerializer(typeof(SettingsXmlModel));
            if (!serializer.CanDeserialize(reader) || serializer.Deserialize(reader) is not SettingsXmlModel settingsXmlModel)
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
                new List<ISettingsPresetBuilder>())
            {
                FilePath = filePath
            };
        }

        public string FilePath { get; init; }

        private ExternalGlobalSettings(string id, string displayName, string folderName, string subFolder, string format, int uiVersion, char subGroupDelimiter, PropertyChangedEventHandler? onPropertyChanged, IEnumerable<SettingsPropertyGroupDefinition> settingPropertyGroups, IEnumerable<ISettingsPresetBuilder> presets)
            : base(id, displayName, folderName, subFolder, format, uiVersion, subGroupDelimiter, onPropertyChanged, settingPropertyGroups, presets) { }
    }
}