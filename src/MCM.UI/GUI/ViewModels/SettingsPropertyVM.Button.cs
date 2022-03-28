using MCM.UI.Actions;

using TaleWorlds.Library;

namespace MCM.UI.GUI.ViewModels
{
    internal sealed partial class SettingsPropertyVM : ViewModel
    {
        [DataSourceProperty]
        public bool IsButton { get; }

        [DataSourceProperty]
        public string ButtonContent => SettingPropertyDefinition.Content;
    }
}