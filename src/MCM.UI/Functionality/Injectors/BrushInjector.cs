using Bannerlord.ButterLib.Common.Helpers;

using HarmonyLib;

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Xml;

using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI;

namespace MCM.UI.Functionality.Injectors
{
    internal static class BrushInjector
    {
        private delegate Brush LoadBrushFromDelegate(BrushFactory instance, XmlNode brushNode);

        private static readonly LoadBrushFromDelegate? LoadBrushFrom =
            AccessTools2.GetDelegate<LoadBrushFromDelegate>(typeof(BrushFactory), "LoadBrushFrom");

        private static readonly AccessTools.FieldRef<BrushFactory, Dictionary<string, Brush>>? GetBrushes =
            AccessTools2.FieldRefAccess<BrushFactory, Dictionary<string, Brush>>("_brushes");

        internal static readonly ConcurrentDictionary<Brush, object?> Brushes = new ConcurrentDictionary<Brush, object?>();

        public static void InjectDocument(XmlDocument xmlDocument)
        {
            if (GetBrushes != null)
            {
                var brushes = GetBrushes(UIResourceManager.BrushFactory);
                foreach (XmlNode brushNode in xmlDocument.SelectSingleNode("Brushes")!.ChildNodes)
                {
                    var brush = LoadBrushFrom?.Invoke(UIResourceManager.BrushFactory, brushNode);
                    if (brush != null)
                    {
                        brushes[brush.Name] = brush;
                        Brushes.TryAdd(brush, null);
                    }
                }
            }
        }
    }
}