using Bannerlord.BUTR.Shared.Helpers;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Settings.Base.Global;

using Microsoft.Extensions.Logging;

using System.Collections.Generic;

using TaleWorlds.Localization;

namespace MCM.UI
{
    internal sealed class MCMUISettings : AttributeGlobalSettings<MCMUISettings>
    {
        private bool _useStandardOptionScreen;

        public override string Id { get; } = "MCMUI_v4";
        public override string DisplayName => TextObjectHelper.Create("{=MCMUISettings_Name}MCM UI {VERSION}", new Dictionary<string, TextObject?>
        {
            { "VERSION", TextObjectHelper.Create(typeof(MCMUISettings).Assembly.GetName().Version?.ToString(3) ?? "ERROR") }
        })?.ToString() ?? "ERROR";
        public override string FolderName { get; } = "MCM";
        public override string FormatType { get; } = "json";

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
    }
}