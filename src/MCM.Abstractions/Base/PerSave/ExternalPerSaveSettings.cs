using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.Xml;
using MCM.Common;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml;

namespace MCM.Abstractions.Base.PerSave
{
    /// <summary>
    /// Per save settings declared in XML (<see cref="SettingsXmlModel"/>). The caller supplies the value
    /// references, the XML supplies the layout.
    /// </summary>
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
        /// <summary>
        /// Reads the first <c>&lt;Settings&gt;</c> block of the stream, a standalone settings file or a <c>SubModule.xml</c>.
        /// </summary>
        /// <param name="assignRefDelegate">Creates the reference a property reads and writes. See <see cref="Global.ExternalGlobalSettings.CreateReference"/> for one holding the declared default.</param>
        public static ExternalPerSaveSettings? CreateFromXmlStream(Stream xmlStream, Func<IPropertyDefinitionBase, IRef> assignRefDelegate, PropertyChangedEventHandler? propertyChanged = null)
        {
            var document = new XmlDocument();
            document.Load(xmlStream);
            return SettingsXmlModel.FromDocument(document).FirstOrDefault() is { } model ? Create(model, assignRefDelegate, propertyChanged) : null;
        }

        public static ExternalPerSaveSettings Create(SettingsXmlModel model, Func<IPropertyDefinitionBase, IRef> assignRefDelegate, PropertyChangedEventHandler? propertyChanged = null)
        {
            var delimiter = model.SubGroupDelimiterChar;
            var properties = model.GetGroupedProperties()
                .Select(x => new SettingsPropertyDefinition(SettingsUtils.GetPropertyDefinitionWrappers(x.Property), x.Group, assignRefDelegate(x.Property), delimiter));

            return new ExternalPerSaveSettings(
                model.Id,
                model.DisplayName,
                string.Empty,
                string.Empty,
                model.UIVersion,
                delimiter,
                propertyChanged,
                SettingsUtils.GetSettingsPropertyGroups(delimiter, properties),
                new List<ISettingsPresetBuilder>());
        }

        private ExternalPerSaveSettings(string id, string displayName, string folderName, string subFolder, int uiVersion, char subGroupDelimiter, PropertyChangedEventHandler? onPropertyChanged, IEnumerable<SettingsPropertyGroupDefinition> settingPropertyGroups, IEnumerable<ISettingsPresetBuilder> presets)
            : base(id, displayName, folderName, subFolder, uiVersion, subGroupDelimiter, onPropertyChanged, settingPropertyGroups, presets)
        {
        }
    }
}
