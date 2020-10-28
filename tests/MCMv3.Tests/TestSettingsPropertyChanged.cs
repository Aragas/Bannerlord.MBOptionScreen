using MCM.Abstractions.Attributes.v2;

namespace MCMv3.Tests
{
    internal sealed class TestSettingsPropertyChanged : BaseTestGlobalSettings<TestSettingsOrder>
    {
        public override string Id => "Testing_PropertyChanged_v3";
        public override string DisplayName => "MCMv3 Testing PropertyChanged";


        private bool _property1 = true;
        [SettingPropertyBool("Property 1", RequireRestart = false)]
        public bool Property1
        {
            get => _property1;
            set
            {
                if (_property1 != value)
                {
                    _property1 = value;
                    OnPropertyChanged(nameof(Property1));
                }
            }
        }

        private bool _property2;
        [SettingPropertyBool("Property 2", RequireRestart = false)]
        public bool Property2
        {
            get => _property2;
            set
            {
                if (_property2 != value)
                {
                    _property2 = value;
                    OnPropertyChanged(nameof(Property2));
                }
            }
        }

        public TestSettingsPropertyChanged()
        {
            PropertyChanged += TestSettingsPropertyChanged_PropertyChanged;
        }

        private void TestSettingsPropertyChanged_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Property2))
            {
                Property1 = !Property2;
            }
        }
    }
}