using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.Utils;
using MCM.Abstractions.Xml;
using MCM.Common;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace MCM.Abstractions.Base.PerSave
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    sealed class ExternalPerSaveSettings : FluentPerSaveSettings
    {
        public static ExternalPerSaveSettings? CreateFromXmlStream(Stream xmlStream, Func<IPropertyDefinitionBase, IRef> assignRefDelegate, PropertyChangedEventHandler? propertyChanged = null)
        {
            if (SerializationUtils.DeserializeXml<SettingsXmlModel>(xmlStream) is not { } settingsXmlModel)
                return null;

            var subGroupDelimiter = settingsXmlModel.SubGroupDelimiter[0];

            var props = settingsXmlModel.Groups
                .SelectMany(g => g.Properties.Select(p =>
                    new SettingsPropertyDefinition(SettingsUtils.GetPropertyDefinitionWrappers(p), g, assignRefDelegate(p), subGroupDelimiter)))
                .Concat(settingsXmlModel.Properties.Select(p =>
                    new SettingsPropertyDefinition(SettingsUtils.GetPropertyDefinitionWrappers(p), SettingsPropertyGroupDefinition.DefaultGroup, assignRefDelegate(p), subGroupDelimiter)));
            var propGroups = SettingsUtils.GetSettingsPropertyGroups(subGroupDelimiter, props);

            return new ExternalPerSaveSettings(
                settingsXmlModel.Id,
                settingsXmlModel.DisplayName,
                settingsXmlModel.FolderName,
                settingsXmlModel.SubFolder,
                settingsXmlModel.UIVersion,
                subGroupDelimiter,
                propertyChanged,
                propGroups,
                new List<ISettingsPresetBuilder>());
        }

        private ExternalPerSaveSettings(string id, string displayName, string folderName, string subFolder, int uiVersion, char subGroupDelimiter, PropertyChangedEventHandler? onPropertyChanged, IEnumerable<SettingsPropertyGroupDefinition> settingPropertyGroups, IEnumerable<ISettingsPresetBuilder> presets)
            : base(id, displayName, folderName, subFolder, uiVersion, subGroupDelimiter, onPropertyChanged, settingPropertyGroups, presets)
        {
        }
    }
}