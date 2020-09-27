using Bannerlord.ButterLib.Common.Extensions;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Xml;

using TaleWorlds.GauntletUI.PrefabSystem;

namespace MCM.UI.Functionality
{
    public abstract class BaseResourceHandler
    {
        public static BaseResourceHandler? Instance =>
            MCMSubModule.Instance?.GetServiceProvider()?.GetRequiredService<BaseResourceHandler>();

        public abstract void InjectBrush(XmlDocument xmlDocument);
        public abstract void InjectPrefab(string prefabName, XmlDocument xmlDocument);
        public abstract void InjectWidget(Type widgetType);
        public abstract WidgetPrefab? MovieRequested(string movie);
    }
}