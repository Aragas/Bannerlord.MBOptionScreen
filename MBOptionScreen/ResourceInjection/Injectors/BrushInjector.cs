using HarmonyLib;

using System.Collections;
using System.Reflection;
using System.Xml;

using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI;

namespace MBOptionScreen.ResourceInjection.Injectors
{
    internal static class BrushInjector
    {
        private static readonly AccessTools.FieldRef<BrushFactory, IDictionary> _brushes =
            AccessTools.FieldRefAccess<BrushFactory, IDictionary>("_brushes");
        private static MethodInfo LoadBrushFromMethod { get; } = AccessTools.Method(typeof(BrushFactory), "LoadBrushFrom");

        public static void InjectDocument(XmlDocument xmlDocument)
        {
            var brushes = _brushes(UIResourceManager.BrushFactory);
            foreach (XmlNode brushNode in xmlDocument.SelectSingleNode("Brushes").ChildNodes)
            {
                var brush = (Brush) LoadBrushFromMethod.Invoke(UIResourceManager.BrushFactory, new object[] { brushNode });
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