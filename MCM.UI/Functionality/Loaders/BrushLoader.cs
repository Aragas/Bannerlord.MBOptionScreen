using MCM.Abstractions.Functionality;

using System.Xml;

namespace MCM.UI.ResourceInjection.Loaders
{
    /// <summary>
    /// Loads the embed .xml files from the library
    /// </summary>
    internal static class BrushLoader
    {
        public static XmlDocument ButtonBrushes() => Load("MCM.UI.GUI.Brushes.ButtonBrushes.xml");
        public static XmlDocument DividerBrushes() => Load("MCM.UI.GUI.Brushes.DividerBrushes.xml");
        public static XmlDocument ExpandIndicator() => Load("MCM.UI.GUI.Brushes.ExpandIndicator.xml");
        public static XmlDocument SettingsBrush() => Load("MCM.UI.GUI.Brushes.SettingsBrush.xml");
        public static XmlDocument ResetButtonBrush() => Load("MCM.UI.GUI.Brushes.ResetButtonBrush.xml");
        public static XmlDocument SettingsValueDisplayBrush() => Load("MCM.UI.GUI.Brushes.SettingsValueDisplayBrush.xml");
        public static XmlDocument TextBrushes() => Load("MCM.UI.GUI.Brushes.TextBrushes.xml");

        private static XmlDocument Load(string embedPath)
        {
            using var stream = typeof(PrefabsLoader).Assembly.GetManifestResourceStream(embedPath);
            var doc = new XmlDocument();
            doc.Load(stream);
            return doc;
        }

        public static void Inject(BaseResourceHandler resourceInjector)
        {
            resourceInjector.InjectBrush(ButtonBrushes());
            resourceInjector.InjectBrush(DividerBrushes());
            resourceInjector.InjectBrush(ExpandIndicator());
            resourceInjector.InjectBrush(SettingsBrush());
            resourceInjector.InjectBrush(ResetButtonBrush());
            resourceInjector.InjectBrush(SettingsValueDisplayBrush());
            resourceInjector.InjectBrush(TextBrushes());
        }
    }
}