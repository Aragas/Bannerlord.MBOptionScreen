using MBOptionScreen.ResourceInjection;

using System.Xml;

namespace MBOptionScreen.GUI.v1d.ResourceInjection
{
    /// <summary>
    /// Loads the embed .xml files from the library
    /// </summary>
    internal static class BrushLoader
    {
        private static readonly string ButtonBrushesPath = "MBOptionScreen.GUI.v1d.Brushes.ButtonBrushes.xml";
        private static readonly string DividerBrushesPath = "MBOptionScreen.GUI.v1d.Brushes.DividerBrushes.xml";
        private static readonly string ExpandIndicatorPath = "MBOptionScreen.GUI.v1d.Brushes.ExpandIndicator.xml";
        private static readonly string ModSettingsBrushPath = "MBOptionScreen.GUI.v1d.Brushes.ModSettingsBrush.xml";
        private static readonly string ResetButtonBrushPath = "MBOptionScreen.GUI.v1d.Brushes.ResetButtonBrush.xml";
        private static readonly string SettingsValueDisplayBrushPath = "MBOptionScreen.GUI.v1d.Brushes.SettingsValueDisplayBrush.xml";
        private static readonly string TextBrushesPath = "MBOptionScreen.GUI.v1d.Brushes.TextBrushes.xml";

        public static XmlDocument ButtonBrushes() => Load(ButtonBrushesPath);
        public static XmlDocument DividerBrushes() => Load(DividerBrushesPath);
        public static XmlDocument ExpandIndicator() => Load(ExpandIndicatorPath);
        public static XmlDocument ModSettingsBrush() => Load(ModSettingsBrushPath);
        public static XmlDocument ResetButtonBrush() => Load(ResetButtonBrushPath);
        public static XmlDocument SettingsValueDisplayBrush() => Load(SettingsValueDisplayBrushPath);
        public static XmlDocument TextBrushes() => Load(TextBrushesPath);

        private static XmlDocument Load(string embedPath)
        {
            using var stream = typeof(PrefabsLoader).Assembly.GetManifestResourceStream(embedPath);
            var doc = new XmlDocument();
            doc.Load(stream);
            return doc;
        }

        public static void Inject(BaseResourceInjector resourceInjector)
        {
            resourceInjector.InjectBrush(ButtonBrushes());
            resourceInjector.InjectBrush(DividerBrushes());
            resourceInjector.InjectBrush(ExpandIndicator());
            resourceInjector.InjectBrush(ModSettingsBrush());
            resourceInjector.InjectBrush(ResetButtonBrush());
            resourceInjector.InjectBrush(SettingsValueDisplayBrush());
            resourceInjector.InjectBrush(TextBrushes());
        }
    }
}