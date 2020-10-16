Example settings.  
Check [this page](https://github.com/Aragas/Bannerlord.MBOptionScreen/wiki/Attributes) for specific information on the attributes.
```csharp
    public class CustomSettings : AttributeSettings<CustomSettings>
    {        
        // I recommend to use format "INTERNALMODNAME_v1", e.g. "Aragas.MercenaryContract_v1"
        // You SHOULD change the version if you change the setting class in a non backwards-compatible way
        public override string Id { get; set; } = "Aragas.MercenaryContract_v1";
        // Your mod name in human readable format, e.g. "Limited Mercenary Contract"
        public override string ModName => "Your Mod Name";
        // The name of the folder within which the settings will be saved, e.g. "Aragas.MercenaryContract"
        public override string ModuleFolderName => "Name_of_Folder_Used_for_Saving_Settings";

        // Actual configuration definition with the v1 API
        // v2 API is highly recommended and v1 API is annotated obsolete in latest version (This line add by SaulHE)
        [SettingProperty("Enabled", "")]
        [SettingPropertyGroup("General")]
        public bool Enabled { get; set; } = true;

        [SettingProperty("Apply Relationship Rules to NPC", "")]
        [SettingPropertyGroup("General")]
        public bool ApplyRelationshipRulesToNPC { get; set; } = false;

        [SettingProperty("Independent Multiplier", 1, 4, "Relationship multiplier when Hero is independent.")]
        [SettingPropertyGroup("Multipliers")]
        public int IndependentMultiplier { get; set; } = 1;

        [SettingProperty("Mercenary Multiplier", 1, 4, "Relationship multiplier when Hero is a mercenary.")]
        [SettingPropertyGroup("Multipliers")]
        public int MercenaryMultiplier { get; set; } = 1;

        [SettingProperty("Vassal Multiplier", 1, 4, "Relationship multiplier when Hero is a vassal.")]
        [SettingPropertyGroup("Multipliers")]
        public int VassalMultiplier { get; set; } = 2;

	// Configuration definition with the v2 API
        [SettingPropertyBool(displayName: "Test Bool", order: -1, requireRestart: false, hintText: "This is a v2 bool value definition")]
        [SettingPropertyGroup("v2 Test")]
        public bool TestBool { get; set; } = true;

        [SettingPropertyFloatingInteger(displayName: "Test Float", minValue: 0f, maxValue: 1f, requireRestart: false, hintText: "A v2 float definition formatted to a percentage display", valueFormat: "#0%")]
        [SettingPropertyGroup("v2 Test")]
        public float TestFloat { get; set; } = 0.5f;

        [SettingPropertyInteger(displayName: "Test Int", minValue: 0, maxValue: 50, requireRestart: false, hintText: "A v2 float definition with 'Denars' appended to the value", valueFormat: "0 Denars")]
        [SettingPropertyGroup("v2 Test")]
        public int TestInt { get; set; } = 20;

        [SettingPropertyText(displayName: "Test Text", order: -1, requireRestart: false, hintText: "A v2 textbox definition")]
        [SettingPropertyGroup("v2 Test")]
        public string TestText { get; set; } = "Test";
    }
```

To get the loaded setting in your mod, use the static property ``CustomSettings.Instance``.

To understand how to format the strings take a look at [this](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-numeric-format-strings) page for information and examples (specifically the ToString version). 

Different than ModLib, You don't have to do anything except for implementing your own custom setting class.

MBOptionScreen will automatically scan any derived classes of `AttributeSetting` class.

