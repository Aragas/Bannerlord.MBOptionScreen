using MBOptionScreen.Interfaces;

using System.Xml;

namespace MBOptionScreen.ResourceInjection.EmbedLoaders
{
    /// <summary>
    /// Loads the embed .xml files from the library
    /// </summary>
    internal static class BrushLoaderV1a
    {
        private static readonly string ButtonBrushesPath = "MBOptionScreen.GUI.v1a.Brushes.ButtonBrushes.xml";
        private static readonly string DividerBrushesPath = "MBOptionScreen.GUI.v1a.Brushes.DividerBrushes.xml";
        private static readonly string ExpandIndicatorPath = "MBOptionScreen.GUI.v1a.Brushes.ExpandIndicator.xml";
        private static readonly string ModSettingsItemBrushPath = "MBOptionScreen.GUI.v1a.Brushes.ModSettingsItemBrush.xml";
        private static readonly string ResetButtonBrushPath = "MBOptionScreen.GUI.v1a.Brushes.ResetButtonBrush.xml";
        private static readonly string SettingsValueDisplayBrushPath = "MBOptionScreen.GUI.v1a.Brushes.SettingsValueDisplayBrush.xml";
        private static readonly string TextBrushesPath = "MBOptionScreen.GUI.v1a.Brushes.TextBrushes.xml";

        public static XmlDocument ButtonBrushes() => Load(ButtonBrushesPath);
        public static XmlDocument DividerBrushes() => Load(DividerBrushesPath);
        public static XmlDocument ExpandIndicator() => Load(ExpandIndicatorPath);
        public static XmlDocument ModSettingsItemBrush() => Load(ModSettingsItemBrushPath);
        public static XmlDocument ResetButtonBrush() => Load(ResetButtonBrushPath);
        public static XmlDocument SettingsValueDisplayBrush() => Load(SettingsValueDisplayBrushPath);
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
            resourceInjector.InjectBrush(ButtonBrushes());
            resourceInjector.InjectBrush(DividerBrushes());
            resourceInjector.InjectBrush(ExpandIndicator());
            resourceInjector.InjectBrush(ModSettingsItemBrush());
            resourceInjector.InjectBrush(ResetButtonBrush());
            resourceInjector.InjectBrush(SettingsValueDisplayBrush());
            resourceInjector.InjectBrush(TextBrushes());
        }
    }
}