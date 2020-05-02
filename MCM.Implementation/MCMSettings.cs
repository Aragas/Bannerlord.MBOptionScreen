using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Settings;

namespace MCM.Implementation
{
    internal sealed class MCMSettings : AttributeSettingsBase<MCMSettings>
    {
        public override string Id => "MBOptionScreen_v2";
        public override string ModName => $"Mod Configuration Menu {typeof(MCMSettings).Assembly.GetName().Version.ToString(3)}";
        public override string ModuleFolderName => "";

        [SettingPropertyBool("Override ModLib Option Screen", Order = 2, RequireRestart = true, HintText = "If set, removes ModLib 'Mod Options' menu entry.")]
        [SettingPropertyGroup("General")]
        public bool OverrideModLib { get; set; } = true;
    }
}