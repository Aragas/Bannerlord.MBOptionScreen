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

        public static WidgetPrefab LoadOptionsWithModOptionsView() => PrefabInjector.InjectDocumentAndCreate("OptionsWithModOptionsView", Load("MCM.UI.GUI.Views.OptionsWithModOptionsView.xml"));
        public static WidgetPrefab LoadModOptionsView() => PrefabInjector.InjectDocumentAndCreate("ModOptionsView", Load("MCM.UI.GUI.Views.ModOptionsView.xml"));
        public static WidgetPrefab LoadEditValueView() => PrefabInjector.InjectDocumentAndCreate("EditValueView", Load("MCM.UI.GUI.Views.EditValueView.xml"));

        private static XmlDocument Load(string embedPath)
        {
            using var stream = typeof(PrefabsLoader).Assembly.GetManifestResourceStream(embedPath);
            var doc = new XmlDocument();
            doc.Load(stream);
            return doc;
        }

        public static void Inject(BaseResourceHandler resourceInjector)
        {
            resourceInjector.InjectPrefab("ModOptionsPageView", ModOptionsPageView());
            resourceInjector.InjectPrefab("SettingsItemView", SettingsItemView());
            resourceInjector.InjectPrefab("SettingsPropertyGroupView", SettingsPropertyGroupView());
            resourceInjector.InjectPrefab("SettingsPropertyView", SettingsPropertyView());
            resourceInjector.InjectPrefab("SettingsView", SettingsView());
        }
    }
}