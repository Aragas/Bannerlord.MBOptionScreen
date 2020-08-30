using HarmonyLib;

using MCM.Abstractions.Functionality;
using MCM.UI.Functionality.Injectors;
using MCM.UI.Functionality.Loaders;
using MCM.UI.Patches;

using System;
using System.Xml;

using TaleWorlds.GauntletUI.PrefabSystem;

namespace MCM.UI.Functionality
{
    /// <summary>
    /// Can be easily replaced once a less hacky solution is found
    /// </summary>
    internal sealed class DefaultResourceHandler : BaseResourceHandler
    {
        public DefaultResourceHandler()
        {
            var harmony = new Harmony("bannerlord.mcm.resourcehandler");
            BrushFactoryPatch.Patch(harmony);
            GauntletMoviePatch.Patch(harmony);
        }

        public override void InjectBrush(XmlDocument xmlDocument) => BrushInjector.InjectDocument(xmlDocument);
        public override void InjectPrefab(string prefabName, XmlDocument xmlDocument) => PrefabInjector.InjectDocumentAndCreate(prefabName, xmlDocument);
        public override void InjectWidget(Type widgetType) => WidgetInjector.InjectWidget(widgetType);
        public override WidgetPrefab? MovieRequested(string movie) => movie switch
        {
            "ModOptionsView_MCMv3" => PrefabsLoader.LoadModOptionsView_MCMv3(),
            "EditValueView_MCMv3" => PrefabsLoader.LoadEditValueView_MCMv3(),
            "OptionsWithModOptionsView_MCMv3" => PrefabsLoader.LoadOptionsWithModOptionsView_MCMv3(),
            _ => null
        };
    }
}