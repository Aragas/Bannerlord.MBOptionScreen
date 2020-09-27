using Bannerlord.ButterLib;
using Bannerlord.ButterLib.Common.Extensions;

using Microsoft.Extensions.DependencyInjection;

using System;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;

namespace MCM.UI.Functionality
{
    public abstract class BaseGameMenuScreenHandler
    {
        public static BaseGameMenuScreenHandler Instance =>
            ButterLibSubModule.Instance.GetServiceProvider().GetRequiredService<BaseGameMenuScreenHandler>();

        public abstract void AddScreen(string internalName, int index, Func<ScreenBase?> screenFactory, TextObject text);
        public abstract void RemoveScreen(string internalName);
    }
}