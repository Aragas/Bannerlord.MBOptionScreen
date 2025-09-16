using BUTR.DependencyInjection.Logger;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Common;

using System;

using TaleWorlds.Library;

namespace MCMv5.Tests;

internal sealed class TestSettingsV2 : BaseTestGlobalSettings<TestSettingsV2>
{
    public override string Id => "Testing_v2_v5";
    public override string DisplayName => "MCMv5 Testing v2 API";


    [SettingPropertyBool("Property Bool Default False", RequireRestart = false)]
    [SettingPropertyGroup("Bool")]
    public bool PropertyBoolDefaultFalse { get; set; } = false;
    [SettingPropertyBool("Property Bool Default True", RequireRestart = false)]
    [SettingPropertyGroup("Bool")]
    public bool PropertyBoolDefaultTrue { get; set; } = true;
    [SettingPropertyBool("Property Bool Require Restart")]
    [SettingPropertyGroup("Bool")]
    public bool PropertyBoolRequireRestart { get; set; }
    [SettingPropertyBool("Property Bool With Hint", RequireRestart = false, HintText = "Hint Text")]
    [SettingPropertyGroup("Bool")]
    public bool PropertyBoolWithHint { get; set; }


    [SettingPropertyInteger("Property Int Default 0", 0, 100, RequireRestart = false)]
    [SettingPropertyGroup("Int")]
    public int PropertyIntDefault0 { get; set; } = 0;
    [SettingPropertyInteger("Property Int Default 1", 0, 100, RequireRestart = false)]
    [SettingPropertyGroup("Int")]
    public int PropertyIntDefault1 { get; set; } = 1;
    [SettingPropertyInteger("Property Int Require Restart", 0, 100)]
    [SettingPropertyGroup("Int")]
    public int PropertyIntRequireRestart { get; set; }
    [SettingPropertyInteger("Property Int -10 to 10", -10, 10, RequireRestart = false)]
    [SettingPropertyGroup("Int")]
    public int PropertyInt1to10 { get; set; }
    [SettingPropertyInteger("Property Int With Hint", 0, 100, RequireRestart = false, HintText = "Hint Text")]
    [SettingPropertyGroup("Int")]
    public int PropertyIntWithHint { get; set; }
    [SettingPropertyInteger("Property Int With Format", 0, 100, "0 Denars", RequireRestart = false)]
    [SettingPropertyGroup("Int")]
    public int PropertyIntWithFormat { get; set; }
    [SettingPropertyInteger("Property Int With Custom Formatter", 0, 100, "0 Denars", RequireRestart = false, CustomFormatter = typeof(TestIntFormatter))]
    [SettingPropertyGroup("Int")]
    public int PropertyIntWithCustomFormatter { get; set; }


    [SettingPropertyFloatingInteger("Property Float Default 0f", 0f, 100f, RequireRestart = false)]
    [SettingPropertyGroup("Float")]
    public float PropertyFloatDefault0f { get; set; } = 0f;
    [SettingPropertyFloatingInteger("Property Float Default 1f", 0f, 100f, RequireRestart = false)]
    [SettingPropertyGroup("Float")]
    public float PropertyFloatDefault1f { get; set; } = 1f;
    [SettingPropertyFloatingInteger("Property Float Require Restart", 0f, 100f, RequireRestart = false)]
    [SettingPropertyGroup("Float")]
    public float PropertyFloatRequireRestart { get; set; }
    [SettingPropertyFloatingInteger("Property Float -10 to 10", -10f, 10f)]
    [SettingPropertyGroup("Float")]
    public float PropertyFloat1to10 { get; set; }
    [SettingPropertyFloatingInteger("Property Float With Hint", 0f, 100f, RequireRestart = false, HintText = "Hint Text")]
    [SettingPropertyGroup("Float")]
    public float PropertyFloatWithHint { get; set; }


    [SettingPropertyText("Property Text Default Empty", RequireRestart = false)]
    [SettingPropertyGroup("Text")]
    public string PropertyTextDefaultEmpty { get; set; } = string.Empty;
    [SettingPropertyText("Property Text Default Text", RequireRestart = false)]
    [SettingPropertyGroup("Text")]
    public string PropertyTextDefaultText { get; set; } = "Text";
    [SettingPropertyText("Property Text Require Restart")]
    [SettingPropertyGroup("Text")]
    public string PropertyTextRequireRestart { get; set; } = string.Empty;
    [SettingPropertyText("Property Text With Hint", RequireRestart = false, HintText = "Hint Text")]
    [SettingPropertyGroup("Text")]
    public string PropertyTextWithHint { get; set; } = string.Empty;


    [SettingPropertyDropdown("Property Dropdown SelectedIndex 0", RequireRestart = false)]
    [SettingPropertyGroup("Dropdown")]
    public Dropdown<string> PropertyDropdownSelectedIndex0 { get; set; } = new(new[]
    {
        "Test1",
        "Test2",
        "Test3",
    }, 0);
    [SettingPropertyDropdown("Property Dropdown SelectedIndex 1", RequireRestart = false)]
    [SettingPropertyGroup("Dropdown")]
    public Dropdown<string> PropertyDropdownSelectedIndex1 { get; set; } = new(new[]
    {
        "Test1",
        "Test2",
        "Test3",
    }, 1);
    [SettingPropertyDropdown("Property Dropdown SelectedIndex 2", RequireRestart = false)]
    [SettingPropertyGroup("Dropdown")]
    public Dropdown<string> PropertyDropdownSelectedIndex2 { get; set; } = new(new[]
    {
        "Test1",
        "Test2",
        "Test3",
    }, 2);
    [SettingPropertyDropdown("Property Dropdown Require Restart")]
    [SettingPropertyGroup("Dropdown")]
    public Dropdown<string> PropertyDropdownRequireRestart { get; set; } = new(new[]
    {
        "Test1",
        "Test2",
        "Test3",
    }, 0);
    [SettingPropertyDropdown("Property Dropdown With Hint", RequireRestart = false, HintText = "Hint Text")]
    [SettingPropertyGroup("Dropdown")]
    public Dropdown<string> PropertyDropdownWithHint { get; set; } = new(new[]
    {
        "Test1",
        "Test2",
        "Test3",
    }, 0);
    [SettingPropertyDropdown("Property Dropdown With Localization", RequireRestart = false)]
    [SettingPropertyGroup("Dropdown")]
    public Dropdown<string> PropertyDropdownWithLocalization { get; set; } = new(new[]
    {
        "{=NoPeRandoM}Test1",
        "{=BaseSettings_Default}ERROR",
        "Test3",
    }, 0);
    [SettingPropertyDropdown("Property Dropdown With Enum", RequireRestart = false)]
    [SettingPropertyGroup("Dropdown")]
    public Dropdown<LogLevel> PropertyDropdownWithEnum { get; set; } = new(new[]
    {
        LogLevel.None,
        LogLevel.Warning,
        LogLevel.Critical,
    }, 0);


    [SettingPropertyDropdown("Property Dropdown Custom SelectedIndex 0", RequireRestart = false)]
    [SettingPropertyGroup("Dropdown Custom")]
    public Dropdown<CustomObject> PropertyDropdownCustomSelectedIndex0 { get; set; } = new(new[]
    {
        new CustomObject("Test1"),
        new CustomObject("Test2"),
        new CustomObject("Test3"),
    }, 0);
    [SettingPropertyDropdown("Property Dropdown Custom SelectedIndex 1", RequireRestart = false)]
    [SettingPropertyGroup("Dropdown Custom")]
    public Dropdown<CustomObject> PropertyDropdownCustomSelectedIndex1 { get; set; } = new(new[]
    {
        new CustomObject("Test1"),
        new CustomObject("Test2"),
        new CustomObject("Test3"),
    }, 1);
    [SettingPropertyDropdown("Property Dropdown Custom SelectedIndex 2", RequireRestart = false)]
    [SettingPropertyGroup("Dropdown Custom")]
    public Dropdown<CustomObject> PropertyDropdownCustomSelectedIndex2 { get; set; } = new(new[]
    {
        new CustomObject("Test1"),
        new CustomObject("Test2"),
        new CustomObject("Test3"),
    }, 2);
    [SettingPropertyDropdown("Property Dropdown Custom Require Restart")]
    [SettingPropertyGroup("Dropdown Custom")]
    public Dropdown<CustomObject> PropertyDropdownCustomRequireRestart { get; set; } = new(new[]
    {
        new CustomObject("Test1"),
        new CustomObject("Test2"),
        new CustomObject("Test3"),
    }, 0);
    [SettingPropertyDropdown("Property Dropdown Custom With Hint", RequireRestart = false, HintText = "Hint Text")]
    [SettingPropertyGroup("Dropdown Custom")]
    public Dropdown<CustomObject> PropertyDropdownCustomWithHint { get; set; } = new(new[]
    {
        new CustomObject("Test1"),
        new CustomObject("Test2"),
        new CustomObject("Test3"),
    }, 0);
    public class CustomObject
    {
        private readonly string _value;
        public CustomObject(string value) => _value = value;
        public override string ToString() => _value;
    }

    [SettingPropertyButton("Property Button Default Empty", Content = "Default Empty", RequireRestart = false)]
    [SettingPropertyGroup("Button")]
    public Action PropertyButtonDefaultEmpty { get; set; } = () => { InformationManager.DisplayMessage(new InformationMessage("Default Empty", Color.White)); };
    [SettingPropertyButton("Property Button Default Text", Content = "Default Text", RequireRestart = false)]
    [SettingPropertyGroup("Button")]
    public Action PropertyButtonDefaultText { get; set; } = () => { InformationManager.DisplayMessage(new InformationMessage("Default Text", Color.White)); };
    [SettingPropertyButton("Property Button Require Restart", Content = "Require Restart")]
    [SettingPropertyGroup("Button")]
    public Action PropertyButtonRequireRestart { get; set; } = () => { InformationManager.DisplayMessage(new InformationMessage("Require Restart", Color.White)); };
    [SettingPropertyButton("Property Button With Hint", Content = "With Hint: Hint Text", RequireRestart = false, HintText = "Hint Text")]
    [SettingPropertyGroup("Button")]
    public Action PropertyButtonWithHint { get; set; } = () => { InformationManager.DisplayMessage(new InformationMessage("With Hint: Hint Text", Color.White)); };

    public class TestIntFormatter : IFormatProvider, ICustomFormatter
    {
        public object? GetFormat(Type? formatType) => formatType == typeof(ICustomFormatter) ? this : null;

        public string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            return (-1).ToString();
        }
    }
}