MCMv5 has no issues with it being used as a soft dependency.  
Mod Developers are required to include the ``MCMv5.dll`` library inside their /bin folder.  
If you are familiar with ILRepack or similar tools, you can pack MCMv5.dll inside yor main .dll to avoid some issues like shared MCMv5.dll loading.  
It can Load/Save settings without the Standalone module. The Standalone module is used to provide the UI implementation of MCMv5. You don't need it if the settings are changed only programmatically.  

#### Example of usage:
The recommended approach is to define a provider interface and depending on certain conditions, switch between them either at the constructor stage or dynamically.  
Here is the final settings class that switches between the providers at the constructor stage:
```csharp
public class CustomSettings
{
    private ICustomSettingsProvider _provider;
    public bool OverrideSomething { get => _provider.OverrideSomething; set => _provider.OverrideSomething = value; }

    public Settings()
    {

        if (CustomSettings.Instance is not null)
        {
            _provider = CustomSettings.Instance;
        }
        // CustomSettings.Instance will return null if something unexpected happened.
        else
        {
            _provider = new HardcodedCustomSettings();
        }
    }
}
```

Here is the provider interface definition and two implementations - one that uses MCM and a custom one that has some hardcoded values.
```csharp
public interface ICustomSettingsProvider
{
    bool OverrideSomething { get; set; }
}

public class HardcodedCustomSettings : ICustomSettingsProvider
{
    public bool OverrideSomething { get; set; } = true;
}

public class CustomSettings : AttributeGlobalSettings<CustomSettings>, ICustomSettingsProvider
{
    private bool _overrideSomething = true;

    public override string Id { get; } = "CustomSettings_v1";
    public override string DisplayName => TextObjectHelper.Create("{=CustomSettings_Name}Custom {VERSION}", new Dictionary<string, TextObject>
    {
        { "VERSION", TextObjectHelper.Create(typeof(CustomSettings).Assembly.GetName().Version.ToString(3)) }
    }).ToString();
    public override string FolderName { get; } = "Custom";
    public override string FormatType { get; } = "json";

    [SettingPropertyBool("{=CustomSettings_Override}Override Something", RequireRestart = true, HintText = "{=CustomSettings_OverrideDesc}If set, does something.")]
    [SettingPropertyGroup("{=CustomSettings_General}General")]
    public bool OverrideSomething
    {
        get => _overrideSomething;
        set
        {
            if (_overrideSomething != value)
            {
                _overrideSomething = value;
                OnPropertyChanged();
            }
        }
    }
}
```

Do not forget to add this to your SubModule.xml so MCM can load it's code and do appropriate bootstrapping!
```xml
  ...
  <SubModules>
    <SubModule>
      <Name value="MCMv5" />
      <DLLName value="MCMv5.dll" />
      <SubModuleClassType value="MCM.MCMSubModule" />
      <Tags />
    </SubModule>
    <SubModule>
      <Name value="MCMv5 Basic Implementation" />
      <DLLName value="MCMv5.dll" />
      <SubModuleClassType value="MCM.Internal.MCMImplementationSubModule" />
      <Tags />
    </SubModule>
  </SubModules>
  ...
```