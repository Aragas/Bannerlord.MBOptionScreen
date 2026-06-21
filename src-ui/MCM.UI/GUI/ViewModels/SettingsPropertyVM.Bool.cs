using MCM.Abstractions;
using MCM.UI.Actions;

using TaleWorlds.Library;

namespace MCM.UI.GUI.ViewModels
{
    internal sealed partial class SettingsPropertyVM : ViewModel
    {
        [DataSourceProperty]
        public bool IsBool => SettingType == SettingType.Bool;

        [DataSourceProperty]
        public bool BoolValue
        {
            get => IsBool && PropertyReference.Value is bool val ? val : false;
            set
            {
                if (IsBool && BoolValue != value)
                {
                    URS.Do(new SetValueTypeAction<bool>(PropertyReference, value));
                    OnPropertyChanged(nameof(BoolValue));
                }
            }
        }
    }
}