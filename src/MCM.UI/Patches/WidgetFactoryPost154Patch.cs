using HarmonyLib;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using TaleWorlds.GauntletUI.PrefabSystem;

namespace MCM.UI.Patches
{
    internal static class WidgetFactoryPost154Patch
    {
        private static readonly Dictionary<string, WidgetPrefab> LiveCustomTypes = new Dictionary<string, WidgetPrefab>();
        private static readonly Dictionary<string, int> LiveInstanceTracker = new Dictionary<string, int>();

        private static Func<string, WidgetPrefab?>? _widgetRequested;

        public static void Patch(Harmony harmony, Func<string, WidgetPrefab?> widgetRequested)
        {
            _widgetRequested = widgetRequested;

            harmony.Patch(
                SymbolExtensions.GetMethodInfo((WidgetFactory wf) => wf.GetCustomType(null!)),
                prefix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(WidgetFactoryPost154Patch), nameof(GetCustomTypePrefix))));

            harmony.Patch(
                AccessTools.DeclaredMethod(typeof(WidgetFactory), "OnUnload"),
                prefix: new HarmonyMethod(AccessTools.DeclaredMethod(typeof(WidgetFactoryPost154Patch), nameof(OnUnloadPrefix))));
        }

        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool GetCustomTypePrefix(string typeName, IDictionary ____liveCustomTypes, ref WidgetPrefab __result)
        {
            if (____liveCustomTypes.Contains(typeName))
                return true;

            if (LiveCustomTypes.TryGetValue(typeName, out var liveWidgetPrefab))
            {
                LiveInstanceTracker[typeName]++;
                __result = liveWidgetPrefab;
                return false;
            }

            if (_widgetRequested?.Invoke(typeName) is { } widgetPrefab)
            {
                LiveCustomTypes.Add(typeName, widgetPrefab);
                LiveInstanceTracker[typeName] = 1;

                __result = widgetPrefab;
                return false;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool OnUnloadPrefix(string typeName)
        {
            if (LiveCustomTypes.ContainsKey(typeName))
            {
                LiveInstanceTracker[typeName]--;
                if (LiveInstanceTracker[typeName] == 0)
                {
                    LiveCustomTypes.Remove(typeName);
                    LiveInstanceTracker.Remove(typeName);
                }
                return false;
            }

            return true;
        }
    }
}