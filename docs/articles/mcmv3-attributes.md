Each setting you want to add to the menu has to be marked with an attribute.
You can additionally group settings by adding the @"MCM.Abstractions.Attributes.SettingPropertyGroupAttribute?text=SettingPropertyGroup" attribute. ``'/'`` is used as the default separator.  

Right now, the mod provides these types in the setting menu:
* Bool
* Int Slider / Textbox
* Float Slider / Textbox 
* Textbox
* Dropdown

## v1
With v1 of the API, @"MCM.Abstractions.Attributes.v1.SettingPropertyAttribute?text=SettingProperty" has multiple constructors, each designed for the specific value it represents.
#### Bool
```csharp
[@"MCM.Abstractions.Attributes.v1.SettingPropertyAttribute?text=SettingProperty"("Setting Name", RequireRestart = false, HintText = "Setting explanation.")]
[@"MCM.Abstractions.Attributes.SettingPropertyGroupAttribute?text=SettingPropertyGroup"("Main Group Name/Nested Group Name/Second Nested Group Name")]
public bool SettingVariableName { get; set; } = true;
```

#### Int
```csharp
[SettingProperty("Setting Name", minValue: 0, maxValue: 10, RequireRestart = false, HintText = "Setting explanation.")]
[SettingPropertyGroup("Main Group Name/Nested Group Name/Second Nested Group Name")]
public int SettingVariableName { get; set; } = 5;
```

#### Float
```csharp
[SettingProperty("Setting Name", minValue: 0f, maxValue: 0.5f, RequireRestart = false, HintText = "Setting explanation.")]
[SettingPropertyGroup("Main Group Name/Nested Group Name/Second Nested Group Name")]
public float SettingVariableName { get; set; } = 0.2f;
```

#### String
```csharp
[SettingProperty("Setting Name", RequireRestart = false, HintText = "Setting explanation.")]
[SettingPropertyGroup("Main Group Name/Nested Group Name/Second Nested Group Name")]
public string SettingVariableName { get; set; } = "The textbox data is here";
```

## v2
With v2 of the API, there's multiple types of setting properties to make using them less confusing, and the settings can also be ordered by indexing them. It's also possible to format the display of the numerical values for the int and float sliders.
#### Bool
```csharp
[SettingPropertyBool("Setting Name", Order = 0, RequireRestart = false, HintText = "Setting explanation.")]
[SettingPropertyGroup("Main Group Name/Nested Group Name/Second Nested Group Name")]
public bool SettingVariableName { get; set; } = true;
```

#### Int
```csharp
// Value is displayed as "X Denars"
[SettingPropertyInteger("Setting Name", 0, 100, "0 Denars", Order = 1, RequireRestart = false, HintText = "Setting explanation.")]
[SettingPropertyGroup("Main Group Name/Nested Group Name/Second Nested Group Name")]
public int SettingVariableName { get; set; } = 25;
```

#### Float
```csharp
// Value is displayed as a percentage
[SettingPropertyFloatingInteger("Setting Name", 0f, 1f, "#0%", Order = 2, RequireRestart = false, HintText = "Setting explanation.")]
[SettingPropertyGroup("Main Group Name/Nested Group Name/Second Nested Group Name")]
public bool SettingVariableName { get; set; } = 0.75;
```

#### String
```csharp
// Value is displayed as a percentage
[SettingPropertyText("Setting Name", Order = 3, RequireRestart = false, HintText = "Setting Explanation")]
[SettingPropertyGroup("Main Group Name/Nested Group Name/Second Nested Group Name")]
public string SettingVariableName { get; set; } = "The textbox data is here";
```
To understand how to format the strings take a look at [this](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-numeric-format-strings) page for information and examples (specifically the ToString version).
  
#### Dropdown
```csharp
[SettingPropertyDropdown("Setting Name")]
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
[SettingPropertyDropdown("Setting Name")]
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
