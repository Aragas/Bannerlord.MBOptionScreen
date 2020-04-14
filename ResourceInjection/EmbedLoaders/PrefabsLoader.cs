using System.Xml;

using TaleWorlds.GauntletUI.PrefabSystem;

namespace MBOptionScreen.ResourceInjection
{
    /// <summary>
    /// Loads the embed .xml files from the library
    /// </summary>
    internal static class PrefabsLoader
    {
        private static readonly string ModOptionsScreen_v1Path = "MBOptionScreen.GUI.v1.Views.ModOptionsScreen.xml";

        private static readonly string ModSettingsItem_v1Path = "MBOptionScreen.GUI.v1.Views.ModSettingsItem.xml";

        private static readonly string SettingPropertyGroupView_v1Path = "MBOptionScreen.GUI.v1.Views.SettingPropertyGroupView.xml";

        private static readonly string SettingPropertyView_v1Path = "MBOptionScreen.GUI.v1.Views.SettingPropertyView.xml";

        private static readonly string SettingsView_v1Path = "MBOptionScreen.GUI.v1.Views.SettingsView.xml";


        public static XmlDocument ModSettingsItem_v1() => Load(ModSettingsItem_v1Path);
        public static XmlDocument SettingPropertyView_v1() => Load(SettingPropertyView_v1Path);
        public static XmlDocument SettingPropertyGroupView_v1() => Load(SettingPropertyGroupView_v1Path);
        public static XmlDocument SettingsView_v1() => Load(SettingsView_v1Path);

        public static WidgetPrefab LoadModOptionsScreen_v1() => PrefabInjector.InjectDocumentAndCreate("ModOptionsScreen_v1", Load(ModOptionsScreen_v1Path));

        private static XmlDocument Load(string embedPath)
        {
            using var stream = typeof(PrefabsLoader).Assembly.GetManifestResourceStream(embedPath);
            var doc = new XmlDocument();
            doc.Load(stream);
            return doc;
        }
    }
}