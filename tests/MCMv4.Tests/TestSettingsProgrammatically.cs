using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Dropdown;

namespace MCMv4.Tests
{
    internal sealed class TestSettingsProgrammatically : BaseTestGlobalSettings<TestSettingsProgrammatically>
    {
        public override string Id => "Testing_Programmatically_v4";
        public override string DisplayName => "MCMv4 Testing Programmatically";


        [SettingPropertyBool("Property Bool Switch", RequireRestart = false)]
        [SettingPropertyGroup("Bool")]
        public bool PropertyBoolSwitch
        {
            get => _propertyBoolSwitch;
            set
            {
                _propertyBoolSwitch = value;
                if (_propertyBoolSwitch)
                {
                    PropertyBool = true;
                    OnPropertyChanged(nameof(PropertyBool));
                }
                else
                {
                    PropertyBool = false;
                    OnPropertyChanged(nameof(PropertyBool));
                }
            }
        }
        private bool _propertyBoolSwitch;

        [SettingPropertyBool("Property Bool Default True", RequireRestart = false)]
        [SettingPropertyGroup("Bool")]
        public bool PropertyBool { get; set; }


        [SettingPropertyBool("Property Int Switch", RequireRestart = false)]
        [SettingPropertyGroup("Int")]
        public bool PropertyIntSwitch
        {
            get => _propertyIntSwitch;
            set
            {
                _propertyIntSwitch = value;
                if (_propertyIntSwitch)
                {
                    PropertyInt = 1;
                    OnPropertyChanged(nameof(PropertyInt));
                }
                else
                {
                    PropertyInt = 0;
                    OnPropertyChanged(nameof(PropertyInt));
                }
            }
        }
        private bool _propertyIntSwitch;

        [SettingPropertyInteger("Property Int", 0, 100, RequireRestart = false)]
        [SettingPropertyGroup("Int")]
        public int PropertyInt { get; set; }


        [SettingPropertyBool("Property Float Switch", RequireRestart = false)]
        [SettingPropertyGroup("Float")]
        public bool PropertyFloatSwitch
        {
            get => _propertyFloatSwitch;
            set
            {
                _propertyFloatSwitch = value;
                if (_propertyFloatSwitch)
                {
                    PropertyFloat = 1f;
                    OnPropertyChanged(nameof(PropertyFloat));
                }
                else
                {
                    PropertyFloat = 0f;
                    OnPropertyChanged(nameof(PropertyFloat));
                }
            }
        }
        private bool _propertyFloatSwitch;

        [SettingPropertyFloatingInteger("Property Float", 0f, 100f, RequireRestart = false)]
        [SettingPropertyGroup("Float")]
        public float PropertyFloat { get; set; }


        [SettingPropertyBool("Property Text Switch", RequireRestart = false)]
        [SettingPropertyGroup("Text")]
        public bool PropertyTextSwitch
        {
            get => _propertyTextSwitch;
            set
            {
                _propertyTextSwitch = value;
                if (_propertyTextSwitch)
                {
                    PropertyText = "Test";
                    OnPropertyChanged(nameof(PropertyText));
                }
                else
                {
                    PropertyText = "";
                    OnPropertyChanged(nameof(PropertyText));
                }
            }
        }
        private bool _propertyTextSwitch;

        [SettingPropertyText("Property Text Default Text", RequireRestart = false)]
        [SettingPropertyGroup("Text")]
        public string PropertyText { get; set; } = "";


        [SettingPropertyBool("Property Dropdown MCM Switch", RequireRestart = false)]
        [SettingPropertyGroup("Dropdown MCM")]
        public bool PropertyDropdownMCMSwitch
        {
            get => _propertyDropdownMCMSwitch;
            set
            {
                _propertyDropdownMCMSwitch = value;
                if (_propertyDropdownMCMSwitch)
                {
                    PropertyDropdownMCM.SelectedIndex = 1;
                    OnPropertyChanged(nameof(PropertyDropdownMCM));
                }
                else
                {
                    PropertyDropdownMCM.SelectedIndex = 0;
                    OnPropertyChanged(nameof(PropertyDropdownMCM));
                }
            }
        }
        private bool _propertyDropdownMCMSwitch;

        [SettingPropertyDropdown("Property Dropdown MCM SelectedIndex", RequireRestart = false)]
        [SettingPropertyGroup("Dropdown MCM")]
        public DropdownMCM<string> PropertyDropdownMCM { get; set; } = new(new[]
        {
            "Test1",
            "Test2",
            "Test3",
        }, 1);


        [SettingPropertyBool("Property Dropdown MCM Custom Switch", RequireRestart = false)]
        [SettingPropertyGroup("Dropdown MCM Custom")]
        public bool PropertyDropdownMCMCustomSwitch
        {
            get => _propertyDropdownMCMCustomSwitch;
            set
            {
                _propertyDropdownMCMCustomSwitch = value;
                if (_propertyDropdownMCMCustomSwitch)
                {
                    PropertyDropdownMCMCustom.SelectedIndex = 1;
                    OnPropertyChanged(nameof(PropertyDropdownMCMCustom));
                }
                else
                {
                    PropertyDropdownMCMCustom.SelectedIndex = 0;
                    OnPropertyChanged(nameof(PropertyDropdownMCMCustom));
                }
            }
        }
        private bool _propertyDropdownMCMCustomSwitch;

        [SettingPropertyDropdown("Property Dropdown MCM Custom", RequireRestart = false)]
        [SettingPropertyGroup("Dropdown MCM Custom")]
        public DropdownMCM<CustomObject> PropertyDropdownMCMCustom { get; set; } = new(new[]
        {
            new CustomObject("Test1"),
            new CustomObject("Test2"),
            new CustomObject("Test3"),
        }, 1);

        
        [SettingPropertyBool("Property Dropdown Default Switch", RequireRestart = false)]
        [SettingPropertyGroup("Dropdown Default")]
        public bool PropertyDropdownDefaultSwitch
        {
            get => _propertyDropdownDefaultSwitch;
            set
            {
                _propertyDropdownDefaultSwitch = value;
                if (_propertyDropdownDefaultSwitch)
                {
                    PropertyDropdownDefault.SelectedIndex = 1;
                    OnPropertyChanged(nameof(PropertyDropdownDefault));
                }
                else
                {
                    PropertyDropdownDefault.SelectedIndex = 0;
                    OnPropertyChanged(nameof(PropertyDropdownDefault));
                }
            }
        }
        private bool _propertyDropdownDefaultSwitch;

        [SettingPropertyDropdown("Property Dropdown Default SelectedIndex", RequireRestart = false)]
        [SettingPropertyGroup("Dropdown Default")]
        public DropdownDefault<string> PropertyDropdownDefault { get; set; } = new(new[]
        {
            "Test1",
            "Test2",
            "Test3",
        }, 1);


        [SettingPropertyBool("Property Dropdown Default Custom Switch", RequireRestart = false)]
        [SettingPropertyGroup("Dropdown Default Custom")]
        public bool PropertyDropdownDefaultCustomSwitch
        {
            get => _propertyDropdownDefaultCustomSwitch;
            set
            {
                _propertyDropdownDefaultCustomSwitch = value;
                if (_propertyDropdownDefaultCustomSwitch)
                {
                    PropertyDropdownDefaultCustom.SelectedIndex = 1;
                    OnPropertyChanged(nameof(PropertyDropdownDefaultCustom));
                }
                else
                {
                    PropertyDropdownDefaultCustom.SelectedIndex = 0;
                    OnPropertyChanged(nameof(PropertyDropdownDefaultCustom));
                }
            }
        }
        private bool _propertyDropdownDefaultCustomSwitch;

        [SettingPropertyDropdown("Property Dropdown Default Custom", RequireRestart = false)]
        [SettingPropertyGroup("Dropdown Default Custom")]
        public DropdownDefault<CustomObject> PropertyDropdownDefaultCustom { get; set; } = new(new[]
        {
            new CustomObject("Test1"),
            new CustomObject("Test2"),
            new CustomObject("Test3"),
        }, 1);
        
        public class CustomObject
        {
            private readonly string _value;
            public CustomObject(string value) => _value = value;
            public override string ToString() => _value;
        }
    }
}