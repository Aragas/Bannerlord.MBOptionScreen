### Warning!
**Dropdown is not supported in v1.1.15 and will be introduced in the v2 release!**
***

Each setting you want to add to the menu has to be marked with an attribute.
You can additionally group settings by adding the ``SettingPropertyGroup`` attribute. ``'/'`` is used as the default separator.  

Right now, the mod provides these types in the setting menu:
* Bool
* Int
* Float
* String
* Dropdown

## v1
With v1 of the API, ``SettingProperty`` has multiple constructors, each designed for the specific value it represents.
#### Bool
```csharp
[SettingProperty(displayName: "Setting Name", requireRestart: false, hintText: "Setting explanation.")]
[SettingPropertyGroup("Main Group Name/Nested Group Name/Second Nested Group Name")]
public bool SettingVariableName { get; set; } = true;
```

#### Int
```csharp
[SettingProperty(displayName: "Setting Name", minValue: 0, maxValue: 10, editableMinValue: 0, editableMaxValue: 100, requireRestart: false, hintText: "Setting explanation.")]
[SettingPropertyGroup("Main Group Name/Nested Group Name/Second Nested Group Name")]
public int SettingVariableName { get; set; } = 5;
```

#### Float
```csharp
[SettingProperty(displayName: "Setting Name", minValue: 0f, maxValue: 0.5f, editableMinValue: 0f, editableMaxValue: 1f, requireRestart: false, hintText: "Setting explanation.")]
[SettingPropertyGroup("Main Group Name/Nested Group Name/Second Nested Group Name")]
public float SettingVariableName { get; set; } = 0.2f;
```

#### String
```csharp
[SettingProperty(displayName: "Setting Name", requireRestart: false, hintText: "Setting explanation.")]
[SettingPropertyGroup("Main Group Name/Nested Group Name/Second Nested Group Name")]
public string SettingVariableName { get; set; } = "The textbox data is here";
```

## v2
With v2 of the API, there's multiple types of setting properties to make using them less confusing, and the settings can also be ordered by indexing them. It's also possible to format the display of the numerical values for the int and float sliders.
#### Bool
```csharp
[SettingPropertyBool(displayName: "Setting Name", order: 0, requireRestart: false, hintText: "Setting explanation.")]
[SettingPropertyGroup("Main Group Name/Nested Group Name/Second Nested Group Name")]
public bool SettingVariableName { get; set; } = true;
```

#### Int
```csharp
// Value is displayed as "X Denars"
[SettingPropertyInteger(displayName: "Setting Name", minValue: 0, maxValue: 100, order: 1, requireRestart: false, hintText: "Setting explanation.", valueFormat: "0 Denars")]
[SettingPropertyGroup("Main Group Name/Nested Group Name/Second Nested Group Name")]
public int SettingVariableName { get; set; } = 25;
```

#### Float
```csharp
// Value is displayed as a percentage
[SettingPropertyFloatingInteger(displayName: "Setting Name", minValue: 0f, maxValue: 1f, order: 2, requireRestart: false, hintText: "Setting explanation.", valueFormat: "#0%")]
[SettingPropertyGroup("Main Group Name/Nested Group Name/Second Nested Group Name")]
public bool SettingVariableName { get; set; } = 0.75;
```

#### String
```csharp
// Value is displayed as a percentage
[SettingPropertyText(displayName: "Setting Name", order: 3, requireRestart: false, hintText: "Setting Explanation")]
[SettingPropertyGroup("Main Group Name/Nested Group Name/Second Nested Group Name")]
public string SettingVariableName { get; set; } = "The textbox data is here";
```
To understand how to format the strings take a look at [this](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-numeric-format-strings) page for information and examples (specifically the ToString version).
  
#### Dropdown
```csharp
[SettingPropertyDropdown("Setting Name", 0)]
[SettingPropertyGroup("Main Group Name/Nested Group Name/Second Nested Group Name")]
public DefaultDropdown<string> SettingVariableName { get; set; } = new DefaultDropdown<string>(new string[]
{
    "Test1",
    "Test2",
    "Test3"
}, 0);
```
It can also use custom classes. Don't forget to override .ToString()!
```csharp
[SettingPropertyDropdown("Setting Name", 0)]
[SettingPropertyGroup("Main Group Name/Nested Group Name/Second Nested Group Name")]
public DefaultDropdown<CustomObject> SettingVariableName { get; set; } = new DefaultDropdown<CustomObject>(new CustomObject[]
{
    new CustomObject("Test1"),
    new CustomObject("Test2"),
    new CustomObject("Test3")
}, 0);
public class CustomObject
{
    private readonly string _value;
    public CustomObject(string value) => _value = value;
    public override string ToString() => _value;
}
```