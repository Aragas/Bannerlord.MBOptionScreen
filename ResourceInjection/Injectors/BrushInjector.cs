using System.Collections;
using System.Reflection;
using System.Xml;

using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI;

namespace MBOptionScreen.ResourceInjection
{
    internal static class BrushInjector
    {
        private static MethodInfo LoadBrushFromMethod { get; } = typeof(BrushFactory)
            .GetMethod("LoadBrushFrom", BindingFlags.Instance | BindingFlags.NonPublic);
        private static FieldInfo BrushesField { get; } = typeof(BrushFactory)
            .GetField("_brushes", BindingFlags.Instance | BindingFlags.NonPublic);

        public static void InjectDocument(XmlDocument xmlDocument)
        {
            var brushes = (IDictionary) BrushesField.GetValue(UIResourceManager.BrushFactory);
            foreach (XmlNode brushNode in xmlDocument.SelectSingleNode("Brushes").ChildNodes)
            {
                var brush = (Brush)LoadBrushFromMethod.Invoke(UIResourceManager.BrushFactory, new object[] { brushNode });
                if (brushes.Contains(brush.Name))
                {
                    brushes[brush.Name] = brush;
                }
                else
                {
                    brushes.Add(brush.Name, brush);
                }
            }
        }
    }
}