namespace MCM.Abstractions.Settings.Formats
{
    public class MemorySettingsFormatWrapper : BaseSettingFormatWrapper, IMemorySettingsFormat
    {
        public MemorySettingsFormatWrapper(object @object) : base(@object) { }
    }
}