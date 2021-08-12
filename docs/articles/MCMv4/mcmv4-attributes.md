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
[SettingProperty("Setting Name", RequireRestart = false, HintText = "Setting explanation.")]
[SettingPropertyGroup("Main Group Name/Nested Group Name/Second Nested Group Name")]
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
#### Bool (@"MCM.Abstractions.Attributes.v2.SettingPropertyBoolAttribute?text=SettingPropertyBool")
```csharp
[SettingPropertyBool("Setting Name", Order = 0, RequireRestart = false, HintText = "Setting explanation.")]
[SettingPropertyGroup("Main Group Name/Nested Group Name/Second Nested Group Name")]
public bool SettingVariableName { get; set; } = true;
```

#### Int (@"MCM.Abstractions.Attributes.v2.SettingPropertyIntegerAttribute?text=SettingPropertyInteger")
```csharp
// Value is displayed as "X Denars"
[SettingPropertyInteger("Setting Name", 0, 100, "0 Denars", Order = 1, RequireRestart = false, HintText = "Setting explanation.")]
[SettingPropertyGroup("Main Group Name/Nested Group Name/Second Nested Group Name")]
public int SettingVariableName { get; set; } = 25;
```

#### Float (@"MCM.Abstractions.Attributes.v2.SettingPropertyFloatingIntegerAttribute?text=SettingPropertyFloatingInteger")
```csharp
// Value is displayed as a percentage
[SettingPropertyFloatingInteger("Setting Name", 0f, 1f, "#0%", Order = 2, RequireRestart = false, HintText = "Setting explanation.")]
[SettingPropertyGroup("Main Group Name/Nested Group Name/Second Nested Group Name")]
public bool SettingVariableName { get; set; } = 0.75;
```

#### String (@"MCM.Abstractions.Attributes.v2.SettingPropertyTextAttribute?text=SettingPropertyText")
```csharp
// Value is displayed as a percentage
[SettingPropertyText("Setting Name", Order = 3, RequireRestart = false, HintText = "Setting Explanation")]
[SettingPropertyGroup("Main Group Name/Nested Group Name/Second Nested Group Name")]
public string SettingVariableName { get; set; } = "The textbox data is here";
```
To understand how to format the strings take a look at [this](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-numeric-format-strings) page for information and examples (specifically the ToString version).
  
#### Dropdown (@"MCM.Abstractions.Attributes.v2.SettingPropertyDropdownAttribute?text=SettingPropertyDropdown")
```csharp
[SettingPropertyDropdown("Setting Name")]
[SettingPropertyGroup("Main Group Name/Nested Group Name/Second Nested Group Name")]
public DropdownDefault <string> SettingVariableName { get; set; } = new DropdownDefault <string>(new string[]
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
public DropdownDefault <CustomObject> SettingVariableName { get; set; } = new DropdownDefault <CustomObject>(new CustomObject[]
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


#### Ordering
You can order properties via the [``Order``](xref:MCM.Abstractions.Attributes.BaseSettingPropertyAttribute#collapsible-MCM_Abstractions_Attributes_BaseSettingPropertyAttribute_Order) attribute property
```csharp
[SettingPropertyText("Setting Name", Order = 3)]
public string SettingVariableName { get; set; } = "The textbox data is here";
```

#### Require Restart
You can the game to restart when the property changes via the [``RequireRestart``](xref:MCM.Abstractions.Attributes.BaseSettingPropertyAttribute#collapsible-MCM_Abstractions_Attributes_BaseSettingPropertyAttribute_RequireRestart) attribute property
```csharp
[SettingPropertyText("Setting Name", RequireRestart = true)]
public string SettingVariableName { get; set; } = "The textbox data is here";
```

#### Hint Text
You can set a description to be displayed when hovering over the setting via the [``HintText``](xref:MCM.Abstractions.Attributes.BaseSettingPropertyAttribute#collapsible-MCM_Abstractions_Attributes_BaseSettingPropertyAttribute_HintText) attribute property
```csharp
[SettingPropertyText("Setting Name", HintText = "This is a Hint")]
public string SettingVariableName { get; set; } = "The textbox data is here";
```

#### Group Order
You can order the setting groups via the [``GroupOrder``](xref:MCM.Abstractions.Attributes.SettingPropertyGroupAttribute#collapsible-MCM_Abstractions_Attributes_SettingPropertyGroupAttribute_GroupOrder) attribute property
```csharp
[SettingPropertyText("Setting Name")]
[SettingPropertyGroup("Main Group Name", GroupOrder = 1)]
public string SettingVariableName { get; set; } = "The textbox data is here";
```

#### Group Toggle
You can make a setting property a group toggle via the [``IsToggle``](xref:MCM.Abstractions.Attributes.SettingPropertyGroupAttribute#collapsible-MCM_Abstractions_Attributes_SettingPropertyGroupAttribute_IsToggle) attribute property
```csharp
[SettingPropertyBool("Main Group Name Toggle", IsToggle = true)]
[SettingPropertyGroup("Main Group Name")]        
public bool SettingVariableName{ get; set; } = false;
```
