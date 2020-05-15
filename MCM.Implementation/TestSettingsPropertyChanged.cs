#if DEBUG
using MCM.Abstractions.Attributes.v2;

namespace MCM.Implementation
{
    internal sealed class TestSettingsPropertyChanged : BaseTestGlobalSettings<TestSettingsOrder>
    {
        public override string Id => "Testing_PropertyChanged_v1";
        public override string DisplayName => "Testing PropertyChanged";


        [SettingPropertyBool("Property 1", RequireRestart = false)]
        public bool Property1 { get; set; } = true;

        private bool _property2;
        [SettingPropertyBool("Property 2", RequireRestart = false)]
        public bool Property2
        {
            get => _property2;
            set
            {
                _property2 = value;
                Property1 = !value;
                OnPropertyChanged(nameof(Property2));
            }
        }
    }
}
#endif