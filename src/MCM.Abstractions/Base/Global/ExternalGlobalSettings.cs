using MCM.Abstractions.Definitions;
using MCM.Abstractions.Definitions.Xml;
using MCM.Abstractions.Models;
using MCM.Abstractions.Utils;
using MCM.Common.Ref;

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
        public static ExternalGlobalSettings? CreateFromXmlStream(Stream xmlStream, Func<IPropertyDefinitionBase, IRef> assignRefDelegate, PropertyChangedEventHandler? propertyChanged = null)
        {
            using var reader = XmlReader.Create(xmlStream, new XmlReaderSettings { IgnoreComments = true, IgnoreWhitespace = true });
            var serializer = new XmlSerializer(typeof(SettingsXmlModel));
            if (!serializer.CanDeserialize(reader) || serializer.Deserialize(reader) is not SettingsXmlModel settingsXmlModel)
                return null;

            var subGroupDelimiter = settingsXmlModel.SubGroupDelimiter[0];

            var props = settingsXmlModel.Groups
                .SelectMany(g => g.Properties.Select(p =>
                    new SettingsPropertyDefinition(SettingsUtils.GetPropertyDefinitionWrappers(p), g, assignRefDelegate(p), subGroupDelimiter)))
                .Concat(settingsXmlModel.Properties.Select(p =>
                    new SettingsPropertyDefinition(SettingsUtils.GetPropertyDefinitionWrappers(p), SettingsPropertyGroupDefinition.DefaultGroup, assignRefDelegate(p), subGroupDelimiter)));
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
                new List<ISettingsPresetBuilder>());
        }

        private ExternalGlobalSettings(string id, string displayName, string folderName, string subFolder, string format, int uiVersion, char subGroupDelimiter, PropertyChangedEventHandler? onPropertyChanged, IEnumerable<SettingsPropertyGroupDefinition> settingPropertyGroups, IEnumerable<ISettingsPresetBuilder> presets)
            : base(id, displayName, folderName, subFolder, format, uiVersion, subGroupDelimiter, onPropertyChanged, settingPropertyGroups, presets) { }
    }
}