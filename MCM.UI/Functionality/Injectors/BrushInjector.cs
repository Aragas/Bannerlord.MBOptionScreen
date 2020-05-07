using HarmonyLib;

using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;
using System.Xml;

using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI;

namespace MCM.UI.ResourceInjection.Injectors
{
    internal static class BrushInjector
    {
        private static MethodInfo LoadBrushFromMethod { get; } = AccessTools.Method(typeof(BrushFactory), "LoadBrushFrom");
        private static FieldInfo BrushesField { get; } = AccessTools.Field(typeof(BrushFactory), "_brushes");

        internal static readonly ConcurrentDictionary<Brush, object?> _brushes = new ConcurrentDictionary<Brush, object?>();

        public static void InjectDocument(XmlDocument xmlDocument)
        {
            var brushes = (IDictionary) BrushesField.GetValue(UIResourceManager.BrushFactory);
            foreach (var brushNode in xmlDocument.SelectSingleNode("Brushes").ChildNodes)
            {
                var brush = (Brush) LoadBrushFromMethod.Invoke(UIResourceManager.BrushFactory, new object[] { brushNode });

                if (brushes.Contains(brush.Name))
                    brushes[brush.Name] = brush;
                else
                    brushes.Add(brush.Name, brush);

                _brushes.TryAdd(brush, null);
            }
        }
    }
}