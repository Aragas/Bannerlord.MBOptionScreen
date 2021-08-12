using Bannerlord.UIExtenderEx.ResourceManager;

using MCM.UI.GUI.Views;

namespace MCM.UI.Functionality.Injectors
{
    internal class ResourceInjectorPost154 : ResourceInjector
    {
        public override void Inject()
        {
            BrushFactoryManager.CreateAndRegister(Load("MCM.UI.GUI.v2.Brushes.ButtonBrushes.xml"));
            BrushFactoryManager.CreateAndRegister(Load("MCM.UI.GUI.v2.Brushes.CompatibilityBrushes.xml"));
            BrushFactoryManager.CreateAndRegister(Load("MCM.UI.GUI.v2.Brushes.DividerBrushes.xml"));
            BrushFactoryManager.CreateAndRegister(Load("MCM.UI.GUI.v2.Brushes.ExpandIndicator.xml"));
            BrushFactoryManager.CreateAndRegister(Load("MCM.UI.GUI.v2.Brushes.SettingsBrush.xml"));
            BrushFactoryManager.CreateAndRegister(Load("MCM.UI.GUI.v2.Brushes.ResetButtonBrush.xml"));
            BrushFactoryManager.CreateAndRegister(Load("MCM.UI.GUI.v2.Brushes.SettingsValueDisplayBrush.xml"));
            BrushFactoryManager.CreateAndRegister(Load("MCM.UI.GUI.v2.Brushes.TextBrushes.xml"));

            WidgetFactoryManager.CreateAndRegister("ModOptionsView_MCM", Load("MCM.UI.GUI.v2.Prefabs.ModOptionsView.xml"));
            WidgetFactoryManager.CreateAndRegister("EditValueView_MCM", Load("MCM.UI.GUI.v2.Prefabs.EditValueView.xml"));
            WidgetFactoryManager.CreateAndRegister("DropdownWithHorizontalControlCheckboxView_v2", Load("MCM.UI.GUI.v2.Prefabs.DropdownWithHorizontalControl.Checkbox.xml"));
            WidgetFactoryManager.CreateAndRegister("ModOptionsPageView_v2", Load("MCM.UI.GUI.v2.Prefabs.ModOptionsPageView.xml"));
            WidgetFactoryManager.CreateAndRegister("SettingsItemView_v2", Load("MCM.UI.GUI.v2.Prefabs.SettingsItemView.xml"));
            WidgetFactoryManager.CreateAndRegister("SettingsPropertyGroupView_v2", Load("MCM.UI.GUI.v2.Prefabs.SettingsPropertyGroupView.xml"));
            WidgetFactoryManager.CreateAndRegister("SettingsPropertyView_v2", Load("MCM.UI.GUI.v2.Prefabs.SettingsPropertyView.xml"));
            WidgetFactoryManager.CreateAndRegister("SettingsView_v2", Load("MCM.UI.GUI.v2.Prefabs.SettingsView.xml"));
            WidgetFactoryManager.Register(typeof(EditValueTextWidget));
        }
    }
}