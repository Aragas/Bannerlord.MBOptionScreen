using System.Xml;

namespace MBOptionScreen.ResourceInjection
{
    /// <summary>
    /// Loads the embed .xml files from the library
    /// </summary>
    internal static class BrushLoader
    {
        private static readonly string DividerBrushes_v1Path = "MBOptionScreen.GUI.v1.Brushes.DividerBrushes.xml";
        private static readonly string ModSettingsItem_v1Brush_v1Path = "MBOptionScreen.GUI.v1.Brushes.ModSettingsItemBrush.xml";
        private static readonly string ResetButtonBrush_v1Path = "MBOptionScreen.GUI.v1.Brushes.ResetButtonBrush.xml";
        private static readonly string TextBrushes_v1Path = "MBOptionScreen.GUI.v1.Brushes.TextBrushes.xml";

        public static XmlDocument DividerBrushes_v1() => Load(DividerBrushes_v1Path);
        public static XmlDocument ModSettingsItem_v1Brush_v1() => Load(ModSettingsItem_v1Brush_v1Path);
        public static XmlDocument ResetButtonBrush_v1() => Load(ResetButtonBrush_v1Path);
        public static XmlDocument TextBrushes_v1() => Load(TextBrushes_v1Path);

        private static XmlDocument Load(string embedPath)
        {
            using var stream = typeof(PrefabsLoader).Assembly.GetManifestResourceStream(embedPath);
            var doc = new XmlDocument();
            doc.Load(stream);
            return doc;
        }
    }
}