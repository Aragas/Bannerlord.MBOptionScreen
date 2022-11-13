## Overview
Fluent Builder gives the ability to create Settings at runtime.  
This can be useful for mod specific sections that should not be displayed if the mod is not loaded.  
  
**MCM** provides the builder interface @"MCM.Abstractions.FluentBuilder.ISettingsBuilder?text=ISettingsBuilder" and a default implementation @"MCM.Abstractions.FluentBuilder.Implementation.DefaultSettingsBuilder?text=DefaultSettingsBuilder".  
  

Any property is set by a ``Set%PropertyName%`` method.  
  
``CreateGroup`` creates a property group where you can define your properties. The default Group name is 'Misc'. Use teh action delegate to configure the property group. 
  
``AddBool`` creates a Bool property.  
``AddInteger`` creates an Integer Slider property.  
``AddFloatingInteger`` creates an Float Slider property.  
``AddText`` creates an Textbox property.  
``AddDropdown`` creates a Dropdown property.  
``AddButton`` creates a Button property.  
``AddCustom`` can add a custom property. The custom property should implement one of the interfaces defined in @"MCM.Abstractions.Settings.Definitions" namespace. Currently there is no way of defining a custom UI Control. One of the possible fixes would be to use UIExtender library.  
 
``CreatePreset`` creates a new Preset.  
``SetPropertyValue`` sets an existing property value.  
 
``BuildAsGlobal`` returns a Global setting instance. Use ``Register`` and ``Unregister`` for MCM to use it.  
``BuildAsPerCharacter`` returns a PerCharacter setting  instance. Use ``Register`` and ``Unregister`` for MCM to use it. The registered settings will be cleared before and after player joins the campaign, so do the register thing when the campaign was already joined in.   

## Notes
You can access the default Preset by using
```csharp
builder.CreatePreset(BaseSettings.DefaultPresetId, BaseSettings.DefaultPresetName, pBuilder => { });
```

## Example
* [TrainingTweak](https://github.com/Aragas/TrainingTweak/blob/fba9bb60cdbd5ff61418f4cea30c625b01cd71de/TroopTraining/Settings.cs) - mimics the standard MCM Global Settings flow.