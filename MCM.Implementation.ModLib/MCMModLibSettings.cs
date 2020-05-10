using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Settings;

namespace MCM.Implementation.ModLib
{
    internal sealed class MCMModLibSettings : AttributeGlobalSettings<MCMModLibSettings>
    {
        private bool _overrideModLib = true;

        public override string Id => "MCMModLib_v3";
        public override string DisplayName => $"MCM ModLib Impl. {typeof(MCMModLibSettings).Assembly.GetName().Version.ToString(3)}";
        public override string FolderName => "MCM";
        public override string Format => "json";

        [SettingPropertyBool("Override ModLib Option Screen", Order = 2, RequireRestart = true, HintText = "If set, removes ModLib 'Mod Options' menu entry.")]
        [SettingPropertyGroup("General")]
        public bool OverrideModLib
        {
            get => _overrideModLib;
            set
            {
                if (_overrideModLib != value)
                {
                    _overrideModLib = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}