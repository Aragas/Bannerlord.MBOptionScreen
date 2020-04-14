using System.Xml;

using TaleWorlds.GauntletUI.PrefabSystem;

namespace MBOptionScreen.ResourceInjection
{
    /// <summary>
    /// Loads the embed .xml files from the library
    /// </summary>
    internal static class PrefabsLoaderV1
    {
        private static readonly string ModOptionsScreenPath = "MBOptionScreen.GUI.v1.Views.ModOptionsScreen.xml";

        private static readonly string ModSettingsItemPath = "MBOptionScreen.GUI.v1.Views.ModSettingsItem.xml";

        private static readonly string SettingPropertyGroupViewPath = "MBOptionScreen.GUI.v1.Views.SettingPropertyGroupView.xml";

        private static readonly string SettingPropertyViewPath = "MBOptionScreen.GUI.v1.Views.SettingPropertyView.xml";

        private static readonly string SettingsViewPath = "MBOptionScreen.GUI.v1.Views.SettingsView.xml";


        public static XmlDocument ModSettingsItem() => Load(ModSettingsItemPath);
        public static XmlDocument SettingPropertyView() => Load(SettingPropertyViewPath);
        public static XmlDocument SettingPropertyGroupView() => Load(SettingPropertyGroupViewPath);
        public static XmlDocument SettingsView() => Load(SettingsViewPath);

        public static WidgetPrefab LoadModOptionsScreen() => PrefabInjector.InjectDocumentAndCreate("ModOptionsScreen_v1", Load(ModOptionsScreenPath));

        private static XmlDocument Load(string embedPath)
        {
            using var stream = typeof(PrefabsLoaderV1).Assembly.GetManifestResourceStream(embedPath);
            var doc = new XmlDocument();
            doc.Load(stream);
            return doc;
        }

        public static void Inject(IResourceInjector resourceInjector)
        {
            resourceInjector.InjectPrefab("ModSettingsItem_v1", ModSettingsItem());
            resourceInjector.InjectPrefab("SettingPropertyGroupView_v1", SettingPropertyGroupView());
            resourceInjector.InjectPrefab("SettingPropertyView_v1", SettingPropertyView());
            resourceInjector.InjectPrefab("SettingsView_v1", SettingsView());
        }
    }

    internal static class PrefabsLoaderV1a
    {
        private static readonly string ModOptionsScreenPath = "MBOptionScreen.GUI.v1a.Views.ModOptionsScreen.xml";

        private static readonly string ModSettingsItemPath = "MBOptionScreen.GUI.v1a.Views.ModSettingsItem.xml";

        private static readonly string SettingPropertyGroupViewPath = "MBOptionScreen.GUI.v1a.Views.SettingPropertyGroupView.xml";

        private static readonly string SettingPropertyViewPath = "MBOptionScreen.GUI.v1a.Views.SettingPropertyView.xml";

        private static readonly string SettingsViewPath = "MBOptionScreen.GUI.v1a.Views.SettingsView.xml";


        public static XmlDocument ModSettingsItem() => Load(ModSettingsItemPath);
        public static XmlDocument SettingPropertyView() => Load(SettingPropertyViewPath);
        public static XmlDocument SettingPropertyGroupView() => Load(SettingPropertyGroupViewPath);
        public static XmlDocument SettingsView() => Load(SettingsViewPath);

        public static WidgetPrefab LoadModOptionsScreen() => PrefabInjector.InjectDocumentAndCreate("ModOptionsScreen_v1a", Load(ModOptionsScreenPath));

        private static XmlDocument Load(string embedPath)
        {
            using var stream = typeof(PrefabsLoaderV1).Assembly.GetManifestResourceStream(embedPath);
            var doc = new XmlDocument();
            doc.Load(stream);
            return doc;
        }

        public static void Inject(IResourceInjector resourceInjector)
        {
            resourceInjector.InjectPrefab("ModSettingsItem_v1a", ModSettingsItem());
            resourceInjector.InjectPrefab("SettingPropertyGroupView_v1a", SettingPropertyGroupView());
            resourceInjector.InjectPrefab("SettingPropertyView_v1a", SettingPropertyView());
            resourceInjector.InjectPrefab("SettingsView_v1a", SettingsView());
        }
    }
}