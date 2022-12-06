using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base;
using MCM.Abstractions.Base.Global;

using TaleWorlds.Localization;

namespace MCM.UI
{
    internal sealed class MCMUISettings : AttributeGlobalSettings<MCMUISettings>
    {
        private bool _useStandardOptionScreen;

        public override string Id => "MCMUI_v4";
        public override string DisplayName => new TextObject("{=MCMUISettings_Name}MCM UI {VERSION}", new()
        {
            { "VERSION", typeof(MCMUISettings).Assembly.GetName().Version?.ToString(3) ?? "ERROR" }
        }).ToString();
        public override string FolderName => "MCM";
        public override string FormatType => "json2";

        [SettingPropertyBool("{=MCMUISettings_Name_HideMainMenuEntry}Hide Main Menu Entry", Order = 1, RequireRestart = false,
            HintText = "{=MCMUISettings_Name_HideMainMenuEntryDesc}Hides MCM's Main Menu 'Mod Options' Menu Entry.")]
        [SettingPropertyGroup("{=MCMUISettings_Name_General}General")]
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

        public override void OnPropertyChanged(string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName is SaveTriggered or LoadingComplete)
            {
                MCMUISubModule.UpdateOptionScreen(this);
            }
        }
    }
}