using MCM.Abstractions.Settings.Formats;

namespace MCM.Implementation.Settings.Formats.Xml
{
    public class XmlSettingsFormatWrapper : BaseSettingFormatWrapper, IXmlSettingsFormat
    {
        public XmlSettingsFormatWrapper(object @object) : base(@object) { }
    }
}