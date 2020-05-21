using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Settings.Base.Global;

namespace MCM.UI
{
    internal sealed class MCMUISettings : AttributeGlobalSettings<MCMUISettings>
    {
        private bool _useStandardOptionScreen = false;

        public override string Id => "MCMUI_v3";
        public override string DisplayName => $"MCM UI Impl. {typeof(MCMUISettings).Assembly.GetName().Version.ToString(3)}";
        public override string FolderName => "MCM";
        public override string Format => "json";

        [SettingPropertyBool("Use Standard Option Screen", Order = 1, RequireRestart = false, HintText = "Use standard Options screen instead of using an external.")]
        [SettingPropertyGroup("General")]
        public bool UseStandardOptionScreen
        {
            get => _useStandardOptionScreen;
            set
            {
                if (_useStandardOptionScreen != value)
                {
                    _useStandardOptionScreen = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}