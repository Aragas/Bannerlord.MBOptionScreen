using HarmonyLib;

using MBOptionScreen.Attributes;

using SandBox.View.Map;

using System;
using System.Threading;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;

namespace MBOptionScreen.Functionality
{
    [Version("e1.0.0",  200)]
    [Version("e1.0.1",  200)]
    [Version("e1.0.2",  200)]
    [Version("e1.0.3",  200)]
    [Version("e1.0.4",  200)]
    [Version("e1.0.5",  200)]
    [Version("e1.0.6",  200)]
    [Version("e1.0.7",  200)]
    [Version("e1.0.8",  200)]
    [Version("e1.0.9",  200)]
    [Version("e1.0.10", 200)]
    [Version("e1.0.11", 200)]
    [Version("e1.1.0",  200)]
    [Version("e1.2.0",  200)]
    public sealed class DefaultIngameMenuScreenHandler : IIngameMenuScreenHandler
    {
        private static int _initialized;

        public DefaultIngameMenuScreenHandler()
        {
            // TODO: Test
            if (Interlocked.Exchange(ref _initialized, 1) == 0)
            {
                new Harmony("bannerlord.mboptionscreen.defaultmapscreeninjection_v1").Patch(
                    original: AccessTools.Method(typeof(MapScreen), "GetEscapeMenuItems"),
                    postfix: BaseMapScreenPatches.Instance.GetEscapeMenuItemsPostfix);
            }
        }

        public void AddScreen(int index, Func<ScreenBase> screenFactory, TextObject text)
        {
            BaseMapScreenPatches.Instance.AddScreen(index, screenFactory, text);
        }
    }
}