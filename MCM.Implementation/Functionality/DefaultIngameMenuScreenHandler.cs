using HarmonyLib;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.Functionality;
using MCM.Implementation.Functionality.Harmony;

using SandBox.View.Map;

using System;
using System.Threading;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;

namespace MCM.Implementation.Functionality
{
    [Version("e1.0.0",  1)]
    [Version("e1.0.1",  1)]
    [Version("e1.0.2",  1)]
    [Version("e1.0.3",  1)]
    [Version("e1.0.4",  1)]
    [Version("e1.0.5",  1)]
    [Version("e1.0.6",  1)]
    [Version("e1.0.7",  1)]
    [Version("e1.0.8",  1)]
    [Version("e1.0.9",  1)]
    [Version("e1.0.10", 1)]
    [Version("e1.0.11", 1)]
    [Version("e1.1.0",  1)]
    [Version("e1.2.0",  1)]
    [Version("e1.2.1",  1)]
    [Version("e1.3.0",  1)]
    public sealed class DefaultIngameMenuScreenHandler : IIngameMenuScreenHandler
    {
        private static int _initialized;

        public DefaultIngameMenuScreenHandler()
        {
            if (Interlocked.Exchange(ref _initialized, 1) == 0)
            {
                new HarmonyLib.Harmony("bannerlord.mcm.mapscreeninjection_v3").Patch(
                    original: AccessTools.Method(typeof(MapScreen), "GetEscapeMenuItems"),
                    postfix: BaseHarmonyPatches.Instance.GetEscapeMenuItems);
            }
        }

        public void AddScreen(int index, Func<ScreenBase> screenFactory, TextObject text)
        {
            BaseHarmonyPatches.Instance.AddScreen(index, screenFactory, text);
        }
    }
}