namespace MCM.Abstractions.Settings.Formats.Memory
{
    public class MemorySettingsFormatWrapper : BaseSettingFormatWrapper, IMemorySettingsFormat
    {
        public MemorySettingsFormatWrapper(object @object) : base(@object) { }
    }
}