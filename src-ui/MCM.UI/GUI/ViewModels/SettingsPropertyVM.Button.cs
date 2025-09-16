using MCM.Abstractions;

using TaleWorlds.Library;

namespace MCM.UI.GUI.ViewModels
{
    internal sealed partial class SettingsPropertyVM : ViewModel
    {
        private string _buttonContent = string.Empty;

        [DataSourceProperty]
        public bool IsButton => SettingType == SettingType.Button;

        [DataSourceProperty]
        public string ButtonContent { get => _buttonContent; set => SetField(ref _buttonContent, value, nameof(ButtonContent)); }
    }
}