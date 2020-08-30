using MCM.Abstractions.Functionality;
using MCM.UI.Functionality.Injectors;

using System.Xml;

using TaleWorlds.GauntletUI.PrefabSystem;

namespace MCM.UI.Functionality.Loaders
{
    /// <summary>
    /// Loads the embed .xml files from the library
    /// </summary>
    internal static class PrefabsLoader
    {
        public static XmlDocument ModOptionsPageView() => Load("MCM.UI.GUI.Views.ModOptionsPageView.xml");
        public static XmlDocument SettingsItemView() => Load("MCM.UI.GUI.Views.SettingsItemView.xml");
        public static XmlDocument SettingsPropertyView() => Load("MCM.UI.GUI.Views.SettingsPropertyView.xml");
        public static XmlDocument SettingsPropertyGroupView() => Load("MCM.UI.GUI.Views.SettingsPropertyGroupView.xml");
        public static XmlDocument SettingsView() => Load("MCM.UI.GUI.Views.SettingsView.xml");
        public static XmlDocument DropdownWithHorizontalControlCheckboxView() => Load("MCM.UI.GUI.Views.DropdownWithHorizontalControl.Checkbox.xml");

        public static WidgetPrefab LoadOptionsWithModOptionsView_MCMv3() => PrefabInjector.InjectDocumentAndCreate("OptionsWithModOptionsView_MCMv3", Load("MCM.UI.GUI.Views.OptionsWithModOptionsView.xml"));
        public static WidgetPrefab LoadModOptionsView_MCMv3() => PrefabInjector.InjectDocumentAndCreate("ModOptionsView_MCMv3", Load("MCM.UI.GUI.Views.ModOptionsView.xml"));
        public static WidgetPrefab LoadEditValueView_MCMv3() => PrefabInjector.InjectDocumentAndCreate("EditValueView_MCMv3", Load("MCM.UI.GUI.Views.EditValueView.xml"));

        private static XmlDocument Load(string embedPath)
        {
            using var stream = typeof(PrefabsLoader).Assembly.GetManifestResourceStream(embedPath);
            using var xmlReader = XmlReader.Create(stream, new XmlReaderSettings { IgnoreComments = true });
            var doc = new XmlDocument();
            doc.Load(xmlReader);
            return doc;
        }

        public static void Inject(BaseResourceHandler resourceInjector)
        {
            resourceInjector.InjectPrefab("DropdownWithHorizontalControlCheckboxView_MCMv3", DropdownWithHorizontalControlCheckboxView());
            resourceInjector.InjectPrefab("ModOptionsPageView_MCMv3", ModOptionsPageView());
            resourceInjector.InjectPrefab("SettingsItemView_MCMv3", SettingsItemView());
            resourceInjector.InjectPrefab("SettingsPropertyGroupView_MCMv3", SettingsPropertyGroupView());
            resourceInjector.InjectPrefab("SettingsPropertyView_MCMv3", SettingsPropertyView());
            resourceInjector.InjectPrefab("SettingsView_MCMv3", SettingsView());
        }
    }
}