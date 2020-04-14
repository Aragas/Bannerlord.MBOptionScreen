using System.Xml;

namespace MBOptionScreen.ResourceInjection
{
    /// <summary>
    /// Loads the embed .xml files from the library
    /// </summary>
    internal static class BrushLoaderV1
    {
        private static readonly string DividerBrushesPath = "MBOptionScreen.GUI.v1.Brushes.DividerBrushes.xml";
        private static readonly string ModSettingsItemBrushPath = "MBOptionScreen.GUI.v1.Brushes.ModSettingsItemBrush.xml";
        private static readonly string ResetButtonBrushPath = "MBOptionScreen.GUI.v1.Brushes.ResetButtonBrush.xml";
        private static readonly string TextBrushesPath = "MBOptionScreen.GUI.v1.Brushes.TextBrushes.xml";

        public static XmlDocument DividerBrushes() => Load(DividerBrushesPath);
        public static XmlDocument ModSettingsItemBrush() => Load(ModSettingsItemBrushPath);
        public static XmlDocument ResetButtonBrush() => Load(ResetButtonBrushPath);
        public static XmlDocument TextBrushes() => Load(TextBrushesPath);

        private static XmlDocument Load(string embedPath)
        {
            using var stream = typeof(PrefabsLoaderV1).Assembly.GetManifestResourceStream(embedPath);
            var doc = new XmlDocument();
            doc.Load(stream);
            return doc;
        }

        public static void Inject(IResourceInjector resourceInjector)
        {
            resourceInjector.InjectBrush(DividerBrushes());
            resourceInjector.InjectBrush(ModSettingsItemBrush());
            resourceInjector.InjectBrush(ResetButtonBrush());
            resourceInjector.InjectBrush(TextBrushes());
        }
    }

    internal static class BrushLoaderV1a
    {
        private static readonly string DividerBrushesPath = "MBOptionScreen.GUI.v1a.Brushes.DividerBrushes.xml";
        private static readonly string ModSettingsItemBrushPath = "MBOptionScreen.GUI.v1a.Brushes.ModSettingsItemBrush.xml";
        private static readonly string ResetButtonBrushPath = "MBOptionScreen.GUI.v1a.Brushes.ResetButtonBrush.xml";
        private static readonly string TextBrushesPath = "MBOptionScreen.GUI.v1a.Brushes.TextBrushes.xml";

        public static XmlDocument DividerBrushes() => Load(DividerBrushesPath);
        public static XmlDocument ModSettingsItemBrush() => Load(ModSettingsItemBrushPath);
        public static XmlDocument ResetButtonBrush() => Load(ResetButtonBrushPath);
        public static XmlDocument TextBrushes() => Load(TextBrushesPath);

        private static XmlDocument Load(string embedPath)
        {
            using var stream = typeof(PrefabsLoaderV1a).Assembly.GetManifestResourceStream(embedPath);
            var doc = new XmlDocument();
            doc.Load(stream);
            return doc;
        }

        public static void Inject(IResourceInjector resourceInjector)
        {
            resourceInjector.InjectBrush(DividerBrushes());
            resourceInjector.InjectBrush(ModSettingsItemBrush());
            resourceInjector.InjectBrush(ResetButtonBrush());
            resourceInjector.InjectBrush(TextBrushes());
        }
    }
}