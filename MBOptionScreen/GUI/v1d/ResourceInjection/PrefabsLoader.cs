using MBOptionScreen.ResourceInjection;
using MBOptionScreen.ResourceInjection.Injectors;

using System.Xml;

using TaleWorlds.GauntletUI.PrefabSystem;

namespace MBOptionScreen.GUI.v1d.ResourceInjection
{
    /// <summary>
    /// Loads the embed .xml files from the library
    /// </summary>
    internal static class PrefabsLoader
    {
        private static readonly string EditValueViewPath = "MBOptionScreen.GUI.v1d.Views.EditValueView.xml";
        private static readonly string ModOptionsViewPath = "MBOptionScreen.GUI.v1d.Views.ModOptionsView.xml";
        private static readonly string ModSettingsViewPath = "MBOptionScreen.GUI.v1d.Views.ModSettingsView.xml";
        private static readonly string SettingPropertyGroupViewPath = "MBOptionScreen.GUI.v1d.Views.SettingPropertyGroupView.xml";
        private static readonly string SettingPropertyViewPath = "MBOptionScreen.GUI.v1d.Views.SettingPropertyView.xml";
        private static readonly string SettingsViewPath = "MBOptionScreen.GUI.v1d.Views.SettingsView.xml";

        public static XmlDocument ModSettingsView() => Load(ModSettingsViewPath);
        public static XmlDocument SettingPropertyView() => Load(SettingPropertyViewPath);
        public static XmlDocument SettingPropertyGroupView() => Load(SettingPropertyGroupViewPath);
        public static XmlDocument SettingsView() => Load(SettingsViewPath);

        public static WidgetPrefab LoadModOptionsView() => PrefabInjector.InjectDocumentAndCreate("ModOptionsView_v1d", Load(ModOptionsViewPath));
        public static WidgetPrefab LoadEditValueView() => PrefabInjector.InjectDocumentAndCreate("EditValueView_v1d", Load(EditValueViewPath));

        private static XmlDocument Load(string embedPath)
        {
            using var stream = typeof(PrefabsLoader).Assembly.GetManifestResourceStream(embedPath);
            var doc = new XmlDocument();
            doc.Load(stream);
            return doc;
        }

        public static void Inject(BaseResourceInjector resourceInjector)
        {
            resourceInjector.InjectPrefab("ModSettingsView_v1d", ModSettingsView());
            resourceInjector.InjectPrefab("SettingPropertyGroupView_v1d", SettingPropertyGroupView());
            resourceInjector.InjectPrefab("SettingPropertyView_v1d", SettingPropertyView());
            resourceInjector.InjectPrefab("SettingsView_v1d", SettingsView());
        }
    }
}