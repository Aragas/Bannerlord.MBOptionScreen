MCMv4 has no issues with it being used as a soft dependency.  
Mod Developers are required to include the ``MCMv4.dll`` library inside their /bin folder.  
MCMv4.dll now includes the implementation details, so it can now Load/Save settings without the Standalone module. The Standalone module is now used to provide the UI implementation of MCMv4. You don't need it if the settings are changed only programmatically.  

#### Example of usage:
The recommended approach is to define a provider interface and depending on certain conditions, switch bewteen them either at the constructor stage or dynamically.  
Here is the final settings class that switches between the providers at the constructor stage:
```csharp
public class CustomSettings
{
    private ICustomSettingsProvider _provider;
    public bool OverrideSomething { get => _provider.OverrideSomething; set => _provider.OverrideSomething = value; }

    public Settings()
    {

        if (CustomSettings.Instance != null)
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
    public override string DisplayName => new TextObject("{=CustomSettings_Name}Custom {VERSION}", new Dictionary<string, TextObject>
    {
        { "VERSION", new TextObject(typeof(CustomSettings).Assembly.GetName().Version.ToString(3)) }
    }).ToString();
    public override string FolderName { get; } = "Custom";
    public override string FormatType { get; } = "json2";

    [SettingPropertyBool("{=CustomSettings_Override}Override Ssomething", RequireRestart = true, HintText = "{=CustomSettings_OverrideDesc}If set, does something.")]
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
