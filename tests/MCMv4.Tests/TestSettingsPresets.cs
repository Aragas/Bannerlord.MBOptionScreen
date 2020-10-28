using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Settings.Base;

using System;
using System.Collections.Generic;

namespace MCMv4.Tests
{
    internal sealed class TestSettingsPresets : BaseTestGlobalSettings<TestSettingsPresets>
    {
        public override string Id => "Testing_Presets_v4";
        public override string DisplayName => "MCMv4 Testing Presets";


        [SettingPropertyBool("Property 1", RequireRestart = false)]
        public bool Property1 { get; set; } = true;

        [SettingPropertyBool("Property 2", RequireRestart = false)]
        public bool Property2 { get; set; } = false;


        public override IDictionary<string, Func<BaseSettings>> GetAvailablePresets()
        {
            var basePresets = base.GetAvailablePresets();
            basePresets.Add("Reverse", () => new TestSettingsPresets()
            {
                Property1 = false,
                Property2 = true
            });
            basePresets.Add("False", () => new TestSettingsPresets()
            {
                Property1 = false,
                Property2 = false
            });
            basePresets.Add("True", () => new TestSettingsPresets()
            {
                Property1 = true,
                Property2 = true
            });
            return basePresets;
        }
    }
}