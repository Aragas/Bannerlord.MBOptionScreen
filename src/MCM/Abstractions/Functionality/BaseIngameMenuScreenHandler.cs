using Bannerlord.ButterLib;
using Bannerlord.ButterLib.Common.Extensions;

using System;

using Microsoft.Extensions.DependencyInjection;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;

namespace MCM.Abstractions.Functionality
{
    public abstract class BaseIngameMenuScreenHandler
    {
        public static BaseIngameMenuScreenHandler Instance =>
            ButterLibSubModule.Instance.GetServiceProvider().GetRequiredService<BaseIngameMenuScreenHandler>();

        public abstract void AddScreen(string internalName, int index, Func<ScreenBase?> screenFactory, TextObject text);
        public abstract void RemoveScreen(string internalName);
    }
}