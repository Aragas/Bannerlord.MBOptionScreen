using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Settings.Base.Global;

using System.Collections.Generic;

using TaleWorlds.Localization;

namespace MCM.UI
{
    internal sealed class MCMUISettings : AttributeGlobalSettings<MCMUISettings>
    {
        private bool _useStandardOptionScreen = false;

        public override string Id => "MCMUI_v3";
        public override string DisplayName => new TextObject("{=Settings_Name}MCM UI Impl. {VERSION}", new Dictionary<string, TextObject>()
        {
            { "VERSION", new TextObject(typeof(MCMUISettings).Assembly.GetName().Version.ToString(3)) }
        }).ToString();
        public override string FolderName => "MCM";
        public override string Format => "json";

        [SettingPropertyBool("{=Settings_UseStandard}Use Standard Option Screen", Order = 1, RequireRestart = false, HintText = "{=Settings_UseStandardDesc}Use standard Options screen instead of using an external.")]
        [SettingPropertyGroup("{=Settings_General}General")]
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