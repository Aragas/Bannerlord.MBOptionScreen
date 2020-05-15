using MCM.Abstractions.Settings.Formats;

namespace MCM.Implementation.Settings.Formats
{
    public class XmlSettingsFormatWrapper : BaseSettingFormatWrapper, IXmlSettingsFormat
    {
        public XmlSettingsFormatWrapper(object @object) : base(@object) { }
    }
}