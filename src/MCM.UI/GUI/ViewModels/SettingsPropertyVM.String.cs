using MCM.Abstractions;
using MCM.UI.Actions;

using TaleWorlds.Library;

namespace MCM.UI.GUI.ViewModels
{
    internal sealed partial class SettingsPropertyVM : ViewModel
    {
        [DataSourceProperty]
        public bool IsString => SettingType == SettingType.String;

        [DataSourceProperty]
        public string StringValue
        {
            get => IsString ? PropertyReference.Value is string val ? val : "ERROR" : string.Empty;
            set
            {
                if (IsString && StringValue != value)
                {
                    URS.Do(new SetStringAction(PropertyReference, value));
                    OnPropertyChanged(nameof(StringValue));
                }
            }
        }
    }
}