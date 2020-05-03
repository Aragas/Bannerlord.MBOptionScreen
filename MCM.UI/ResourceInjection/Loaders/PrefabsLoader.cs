using MCM.Abstractions.ResourceInjection;
using MCM.UI.ResourceInjection.Injectors;

using System.Xml;

using TaleWorlds.GauntletUI.PrefabSystem;

namespace MCM.UI.ResourceInjection.Loaders
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

        public static WidgetPrefab LoadOptionsWithModOptionsView() => PrefabInjector.InjectDocumentAndCreate("OptionsWithModOptionsView_v3", Load("MCM.UI.GUI.Views.OptionsWithModOptionsView.xml"));
        public static WidgetPrefab LoadModOptionsView() => PrefabInjector.InjectDocumentAndCreate("ModOptionsView_v3", Load("MCM.UI.GUI.Views.ModOptionsView.xml"));
        public static WidgetPrefab LoadEditValueView() => PrefabInjector.InjectDocumentAndCreate("EditValueView_v3", Load("MCM.UI.GUI.Views.EditValueView.xml"));

        private static XmlDocument Load(string embedPath)
        {
            using var stream = typeof(PrefabsLoader).Assembly.GetManifestResourceStream(embedPath);
            var doc = new XmlDocument();
            doc.Load(stream);
            return doc;
        }

        public static void Inject(BaseResourceInjector resourceInjector)
        {
            resourceInjector.InjectPrefab("ModOptionsPageView_v3", ModOptionsPageView());
            resourceInjector.InjectPrefab("SettingsItemView_v3", SettingsItemView());
            resourceInjector.InjectPrefab("SettingsPropertyGroupView_v3", SettingsPropertyGroupView());
            resourceInjector.InjectPrefab("SettingsPropertyView_v3", SettingsPropertyView());
            resourceInjector.InjectPrefab("SettingsView_v3", SettingsView());
        }
    }
}