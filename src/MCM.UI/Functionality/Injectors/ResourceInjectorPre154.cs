using Bannerlord.UIExtenderEx.ResourceManager;

using MCM.UI.GUI.Views;

namespace MCM.UI.Functionality.Injectors
{
    internal class ResourceInjectorPre154 : ResourceInjector
    {
        public override void Inject()
        {
            BrushFactoryManager.CreateAndRegister(Load("MCM.UI.GUI.v1.Brushes.ButtonBrushes.xml"));
            BrushFactoryManager.CreateAndRegister(Load("MCM.UI.GUI.v1.Brushes.DividerBrushes.xml"));
            BrushFactoryManager.CreateAndRegister(Load("MCM.UI.GUI.v1.Brushes.ExpandIndicator.xml"));
            BrushFactoryManager.CreateAndRegister(Load("MCM.UI.GUI.v1.Brushes.SettingsBrush.xml"));
            BrushFactoryManager.CreateAndRegister(Load("MCM.UI.GUI.v1.Brushes.ResetButtonBrush.xml"));
            BrushFactoryManager.CreateAndRegister(Load("MCM.UI.GUI.v1.Brushes.SettingsValueDisplayBrush.xml"));
            BrushFactoryManager.CreateAndRegister(Load("MCM.UI.GUI.v1.Brushes.TextBrushes.xml"));

            WidgetFactoryManager.CreateAndRegister("ModOptionsView_MCM", Load("MCM.UI.GUI.v1.Prefabs.ModOptionsView.xml"));
            WidgetFactoryManager.CreateAndRegister("EditValueView_MCM", Load("MCM.UI.GUI.v1.Prefabs.EditValueView.xml"));
            WidgetFactoryManager.CreateAndRegister("DropdownWithHorizontalControlCheckboxView_v1", Load("MCM.UI.GUI.v1.Prefabs.DropdownWithHorizontalControl.Checkbox.xml"));
            WidgetFactoryManager.CreateAndRegister("ModOptionsPageView_v1", Load("MCM.UI.GUI.v1.Prefabs.ModOptionsPageView.xml"));
            WidgetFactoryManager.CreateAndRegister("SettingsItemView_v1", Load("MCM.UI.GUI.v1.Prefabs.SettingsItemView.xml"));
            WidgetFactoryManager.CreateAndRegister("SettingsPropertyGroupView_v1", Load("MCM.UI.GUI.v1.Prefabs.SettingsPropertyGroupView.xml"));
            WidgetFactoryManager.CreateAndRegister("SettingsPropertyView_v1", Load("MCM.UI.GUI.v1.Prefabs.SettingsPropertyView.xml"));
            WidgetFactoryManager.CreateAndRegister("SettingsView_v1", Load("MCM.UI.GUI.v1.Prefabs.SettingsView.xml"));
            WidgetFactoryManager.Register(typeof(EditValueTextWidget));
        }
    }
}