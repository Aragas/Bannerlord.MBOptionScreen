MCMv3 has a bug that requires a small workaround for it to be used as a soft dependency.  
Mod Developers are required to include the ``MCMv3.dll`` library inside their /bin folder.  

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
        var useMCM = true;
        try
        {
            // CustomSettings.Instance will throw an exception instead of returning null.
            // This is fixed in MCMv4
            var instance = CustomSettings.Instance;
        }
        catch(Exception)
        {
            useMCM = false;
        }

        if (false)
        {
            _provider = CustomSettings.Instance;
        
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