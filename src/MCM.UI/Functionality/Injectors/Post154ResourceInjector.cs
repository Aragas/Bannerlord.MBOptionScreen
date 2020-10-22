using HarmonyLib;

using MCM.UI.GUI.Views;
using MCM.UI.Patches;

using System.Xml;

using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.PrefabSystem;

namespace MCM.UI.Functionality.Injectors
{
    internal class Post154ResourceInjector : IResourceInjector
    {
        private static XmlDocument Load(string embedPath)
        {
            using var stream = typeof(Post154ResourceInjector).Assembly.GetManifestResourceStream(embedPath);
            using var xmlReader = XmlReader.Create(stream, new XmlReaderSettings { IgnoreComments = true });
            var doc = new XmlDocument();
            doc.Load(xmlReader);
            return doc;
        }


        private static WidgetPrefab? MovieRequested(string movie) => movie switch
        {
            "ModOptionsView_MCM" => PrefabInjector.InjectDocumentAndCreate("ModOptionsView_v2", Load("MCM.UI.GUI.v2.Prefabs.ModOptionsView.xml")),
            "EditValueView_MCM" => PrefabInjector.InjectDocumentAndCreate("EditValueView_v2", Load("MCM.UI.GUI.v2.Prefabs.EditValueView.xml")),
            "OptionsWithModOptionsView_MCM" => PrefabInjector.InjectDocumentAndCreate("OptionsWithModOptionsView_v2", Load("MCM.UI.GUI.v2.Prefabs.OptionsWithModOptionsView.xml")),
            _ => null
        };

        public Post154ResourceInjector()
        {
            var harmony = new Harmony("bannerlord.mcm.resourcehandler.post154");
            GauntletMoviePatch.Patch(harmony, MovieRequested);

            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v2.Brushes.ButtonBrushes.xml"));
            //BrushInjector.InjectDocument(Load("MCM.UI.GUI.v2.Brushes.DividerBrushes.xml"));
            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v2.Brushes.ExpandIndicator.xml"));
            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v2.Brushes.SettingsBrush.xml"));
            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v2.Brushes.ResetButtonBrush.xml"));
            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v2.Brushes.SettingsValueDisplayBrush.xml"));
            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v2.Brushes.TextBrushes.xml"));

            PrefabInjector.InjectDocumentAndCreate("DropdownWithHorizontalControlCheckboxView_v2", Load("MCM.UI.GUI.v2.Prefabs.DropdownWithHorizontalControl.Checkbox.xml"));
            PrefabInjector.InjectDocumentAndCreate("ModOptionsPageView_v2", Load("MCM.UI.GUI.v2.Prefabs.ModOptionsPageView.xml"));
            PrefabInjector.InjectDocumentAndCreate("SettingsItemView_v2", Load("MCM.UI.GUI.v2.Prefabs.SettingsItemView.xml"));
            PrefabInjector.InjectDocumentAndCreate("SettingsPropertyGroupView_v2", Load("MCM.UI.GUI.v2.Prefabs.SettingsPropertyGroupView.xml"));
            PrefabInjector.InjectDocumentAndCreate("SettingsPropertyView_v2", Load("MCM.UI.GUI.v2.Prefabs.SettingsPropertyView.xml"));
            PrefabInjector.InjectDocumentAndCreate("SettingsView_v2", Load("MCM.UI.GUI.v2.Prefabs.SettingsView.xml"));

            WidgetInjector.InjectWidget(typeof(EditValueTextWidget));
            WidgetInfo.ReLoad();
        }
    }

    /*
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
        public override void InjectPrefab(string prefabName, XmlDocument xmlDocument, bool create = false) => PrefabInjector.InjectDocumentAndCreate(prefabName, xmlDocument, create);
        public override void InjectWidget(Type widgetType) => WidgetInjector.InjectWidget(widgetType);
        public override WidgetPrefab? MovieRequested(string movie) => movie switch
        {
            "ModOptionsView_MCMv3" => PrefabsLoader.LoadModOptionsView_MCMv3(),
            "EditValueView_MCMv3" => PrefabsLoader.LoadEditValueView_MCMv3(),
            "OptionsWithModOptionsView_MCMv3" => PrefabsLoader.LoadOptionsWithModOptionsView_MCMv3(),
            _ => null
        };
    }
    */
}