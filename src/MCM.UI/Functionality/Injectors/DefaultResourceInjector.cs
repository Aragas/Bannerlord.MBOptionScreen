using Bannerlord.UIExtenderEx.ResourceManager;

namespace MCM.UI.Functionality.Injectors
{
    internal class DefaultResourceInjector : ResourceInjector
    {
        public override void Inject()
        {
            BrushFactoryManager.CreateAndRegister(Load("MCM.UI.GUI.Brushes.ButtonBrushes.xml"));
            BrushFactoryManager.CreateAndRegister(Load("MCM.UI.GUI.Brushes.CompatibilityBrushes.xml"));
            BrushFactoryManager.CreateAndRegister(Load("MCM.UI.GUI.Brushes.DividerBrushes.xml"));
            BrushFactoryManager.CreateAndRegister(Load("MCM.UI.GUI.Brushes.ExpandIndicator.xml"));
            BrushFactoryManager.CreateAndRegister(Load("MCM.UI.GUI.Brushes.SettingsBrush.xml"));
            BrushFactoryManager.CreateAndRegister(Load("MCM.UI.GUI.Brushes.TextBrushes.xml"));

            WidgetFactoryManager.CreateAndRegister("ModOptionsView_MCM", Load("MCM.UI.GUI.Prefabs.ModOptionsView.xml"));
            WidgetFactoryManager.CreateAndRegister("DropdownWithHorizontalControlCheckboxView", Load("MCM.UI.GUI.Prefabs.DropdownWithHorizontalControl.Checkbox.xml"));
            WidgetFactoryManager.CreateAndRegister("ModOptionsPageView", Load("MCM.UI.GUI.Prefabs.ModOptionsPageView.xml"));
            WidgetFactoryManager.CreateAndRegister("SettingsItemView", Load("MCM.UI.GUI.Prefabs.SettingsItemView.xml"));
            WidgetFactoryManager.CreateAndRegister("SettingsPropertyGroupView", Load("MCM.UI.GUI.Prefabs.SettingsPropertyGroupView.xml"));
            WidgetFactoryManager.CreateAndRegister("SettingsPropertyView", Load("MCM.UI.GUI.Prefabs.SettingsPropertyView.xml"));
            WidgetFactoryManager.CreateAndRegister("SettingsPropertyDisplayValueView", Load("MCM.UI.GUI.Prefabs.SettingsPropertyDisplayValueView.xml"));
            WidgetFactoryManager.CreateAndRegister("SettingsView", Load("MCM.UI.GUI.Prefabs.SettingsView.xml"));

            WidgetFactoryManager.CreateAndRegister("SettingsPropertyBoolView", Load("MCM.UI.GUI.Prefabs.Properties.SettingsPropertyBoolView.xml"));
            WidgetFactoryManager.CreateAndRegister("SettingsPropertyButtonView", Load("MCM.UI.GUI.Prefabs.Properties.SettingsPropertyButtonView.xml"));
            WidgetFactoryManager.CreateAndRegister("SettingsPropertyCheckboxDropdownView", Load("MCM.UI.GUI.Prefabs.Properties.SettingsPropertyCheckboxDropdownView.xml"));
            WidgetFactoryManager.CreateAndRegister("SettingsPropertyDropdownView", Load("MCM.UI.GUI.Prefabs.Properties.SettingsPropertyDropdownView.xml"));
            WidgetFactoryManager.CreateAndRegister("SettingsPropertyFloatView", Load("MCM.UI.GUI.Prefabs.Properties.SettingsPropertyFloatView.xml"));
            WidgetFactoryManager.CreateAndRegister("SettingsPropertyIntView", Load("MCM.UI.GUI.Prefabs.Properties.SettingsPropertyIntView.xml"));
            WidgetFactoryManager.CreateAndRegister("SettingsPropertyStringView", Load("MCM.UI.GUI.Prefabs.Properties.SettingsPropertyStringView.xml"));
        }
    }
}