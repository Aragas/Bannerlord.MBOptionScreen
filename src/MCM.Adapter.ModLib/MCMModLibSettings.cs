using Bannerlord.BUTR.Shared.Helpers;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Settings.Base.Global;

using System.Collections.Generic;

using TaleWorlds.Localization;

namespace MCM.Adapter.ModLib
{
    internal sealed class MCMModLibSettings : AttributeGlobalSettings<MCMModLibSettings>
    {
        private bool _overrideModLib = true;

        public override string Id { get; } = "MCMModLib_v4";
        public override string DisplayName => TextObjectHelper.Create("{=MCMModLibSettings_Name}MCM ModLib Adapter {VERSION}", new Dictionary<string, TextObject?>
        {
            { "VERSION", TextObjectHelper.Create(typeof(MCMModLibSettings).Assembly.GetName().Version?.ToString(3) ?? "ERROR") }
        })?.ToString() ?? "ERROR";
        public override string FolderName { get; } = "MCM";
        public override string FormatType { get; } = "json";

        [SettingPropertyBool("{=MCMModLibSettings_Override}Override ModLib Option Screen", RequireRestart = true, HintText = "{=MCMModLibSettings_OverrideDesc}If set, removes ModLib 'Mod Options' menu entry.")]
        [SettingPropertyGroup("{=MCMModLibSettings_General}General")]
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