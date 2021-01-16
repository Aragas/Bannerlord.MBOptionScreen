using System;
using System.Xml;

using TaleWorlds.GauntletUI.PrefabSystem;

namespace MCM.Abstractions.Functionality
{
    public sealed class ResourceHandlerWrapper : BaseResourceHandler, IWrapper
    {
        public object Object { get; }
        public bool IsCorrect { get; }

        public ResourceHandlerWrapper(object @object) { }

        public override void InjectBrush(XmlDocument xmlDocument) { }
        public override void InjectPrefab(string prefabName, XmlDocument xmlDocument) { }
        public override void InjectWidget(Type widgetType) { }
        public override WidgetPrefab? MovieRequested(string movie) => null;
    }
}