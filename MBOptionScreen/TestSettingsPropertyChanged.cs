using MBOptionScreen.Attributes.v2;

using System.ComponentModel;

namespace MBOptionScreen
{
    internal sealed class TestSettingsPropertyChanged : TestSettingsBase<TestSettingsOrder>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public override string Id { get; set; } = "Testing_PropertyChanged_v1";
        public override string ModName => "Testing PropertyChanged";


        [SettingPropertyBool("Property 1", RequireRestart = false)]
        public bool Property1 { get; set; } = true;

        private bool _property2;
        [SettingPropertyBool("Property 2", RequireRestart = false)]
        public bool Property2
        {
            get { return _property2; }
            set
            {
                _property2 = value;
                Property1 = !value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Property2)));
            }
        }
    }
}