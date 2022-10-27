using Bannerlord.BUTR.Shared.Helpers;

using MCM.Abstractions.Attributes.v2;

using System;

using TaleWorlds.Library;

namespace MCMv4.Tests
{
    internal sealed class TestSettingsButtons : BaseTestGlobalSettings<TestSettingsButtons>
    {
        public override string Id => "Testing_Buttons_v4";
        public override string DisplayName => "MCMv4 Testing Buttons";
        public override string FormatType { get; } = "json";


        [SettingPropertyButton("Property Button One", Content = "Default Text", RequireRestart = false)]
        public Action PropertyButtonOne { get; set; } = () => { InformationManagerHelper.DisplayMessage("Default Text", Color.White); };

        [SettingPropertyButton("Property Button Two", Content = "Default Text", RequireRestart = true)]
        public Action PropertyButtonTwo { get; set; } = () => { InformationManagerHelper.DisplayMessage("Default Text", Color.White); };
    }
}