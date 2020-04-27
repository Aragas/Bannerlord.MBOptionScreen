using MBOptionScreen.ResourceInjection;
using MBOptionScreen.ResourceInjection.Injectors;

using System.Xml;

using TaleWorlds.GauntletUI.PrefabSystem;

namespace MBOptionScreen.GUI.v1c.ResourceInjection
{
    /// <summary>
    /// Loads the embed .xml files from the library
    /// </summary>
    internal static class PrefabsLoader
    {
        private static readonly string EditValueViewPath = "MBOptionScreen.GUI.v1c.Views.EditValueView.xml";
        private static readonly string ModOptionsViewPath = "MBOptionScreen.GUI.v1c.Views.ModOptionsView.xml";
        private static readonly string ModSettingsViewPath = "MBOptionScreen.GUI.v1c.Views.ModSettingsView.xml";
        private static readonly string SettingPropertyGroupViewPath = "MBOptionScreen.GUI.v1c.Views.SettingPropertyGroupView.xml";
        private static readonly string SettingPropertyViewPath = "MBOptionScreen.GUI.v1c.Views.SettingPropertyView.xml";
        private static readonly string SettingsViewPath = "MBOptionScreen.GUI.v1c.Views.SettingsView.xml";

        public static XmlDocument ModSettingsView() => Load(ModSettingsViewPath);
        public static XmlDocument SettingPropertyView() => Load(SettingPropertyViewPath);
        public static XmlDocument SettingPropertyGroupView() => Load(SettingPropertyGroupViewPath);
        public static XmlDocument SettingsView() => Load(SettingsViewPath);

        public static WidgetPrefab LoadModOptionsView() => PrefabInjector.InjectDocumentAndCreate("ModOptionsView_v1c", Load(ModOptionsViewPath));
        public static WidgetPrefab LoadEditValueView() => PrefabInjector.InjectDocumentAndCreate("EditValueView_v1c", Load(EditValueViewPath));

        private static XmlDocument Load(string embedPath)
        {
            using var stream = typeof(PrefabsLoader).Assembly.GetManifestResourceStream(embedPath);
            var doc = new XmlDocument();
            doc.Load(stream);
            return doc;
        }

        public static void Inject(BaseResourceInjector resourceInjector)
        {
            resourceInjector.InjectPrefab("ModSettingsView_v1c", ModSettingsView());
            resourceInjector.InjectPrefab("SettingPropertyGroupView_v1c", SettingPropertyGroupView());
            resourceInjector.InjectPrefab("SettingPropertyView_v1c", SettingPropertyView());
            resourceInjector.InjectPrefab("SettingsView_v1c", SettingsView());
        }
    }
}