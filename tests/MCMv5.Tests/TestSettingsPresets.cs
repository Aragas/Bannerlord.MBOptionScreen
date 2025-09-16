using MCM.Abstractions;
using MCM.Abstractions.Attributes.v2;

using System.Collections.Generic;

namespace MCMv5.Tests;

internal sealed class TestSettingsPresets : BaseTestGlobalSettings<TestSettingsPresets>
{
    public override string Id => "Testing_Presets_v5";
    public override string DisplayName => "MCMv5 Testing Presets";


    [SettingPropertyBool("Property 1", RequireRestart = false)]
    public bool Property1 { get; set; } = true;

    [SettingPropertyBool("Property 2", RequireRestart = false)]
    public bool Property2 { get; set; } = false;


    public override IEnumerable<ISettingsPreset> GetBuiltInPresets()
    {
        foreach (var preset in base.GetBuiltInPresets())
        {
            yield return preset;
        }

        yield return new MemorySettingsPreset(Id, "reverse", "Reverse", () => new TestSettingsPresets
        {
            Property1 = false,
            Property2 = true
        });

        yield return new MemorySettingsPreset(Id, "false", "False", () => new TestSettingsPresets
        {
            Property1 = false,
            Property2 = false
        });

        yield return new MemorySettingsPreset(Id, "true", "True", () => new TestSettingsPresets
        {
            Property1 = true,
            Property2 = true
        });
    }
}