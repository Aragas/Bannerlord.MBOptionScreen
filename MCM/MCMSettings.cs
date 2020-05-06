using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Settings;

namespace MCM
{
    public sealed class MCMSettings : AttributeGlobalSettings<MCMSettings>
    {
        private bool _useStandardOptionScreen = true;
        private bool _overrideModLib = true;

        public override string Id => "MBOptionScreen_v3";
        public override string DisplayName => $"Mod Configuration Menu {typeof(MCMSettings).Assembly.GetName().Version.ToString(3)}";
        public override string ModuleFolderName => "";

        [SettingPropertyBool("Use Standard Option Screen", Order = 1, RequireRestart = false, HintText = "Use standard Options screen instead of using an external.")]
        [SettingPropertyGroup("General")]
        public bool UseStandardOptionScreen
        {
            get => _useStandardOptionScreen;
            set
            {
                _useStandardOptionScreen = value;
                OnPropertyChanged();
            }
        }

        [SettingPropertyBool("Override ModLib Option Screen", Order = 2, RequireRestart = true,
            HintText = "If set, removes ModLib 'Mod Options' menu entry.")]
        [SettingPropertyGroup("General")]
        public bool OverrideModLib
        {
            get => _overrideModLib;
            set
            {
                _overrideModLib = value;
                OnPropertyChanged();
            }
        }
    }
}