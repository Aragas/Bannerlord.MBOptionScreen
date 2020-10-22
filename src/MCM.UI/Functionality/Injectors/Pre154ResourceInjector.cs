using HarmonyLib;

using MCM.UI.GUI.Views;
using MCM.UI.Patches;

using System.Xml;

using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.PrefabSystem;

namespace MCM.UI.Functionality.Injectors
{
    internal class Pre154ResourceInjector : IResourceInjector
    {
        private static XmlDocument Load(string embedPath)
        {
            using var stream = typeof(Pre154ResourceInjector).Assembly.GetManifestResourceStream(embedPath);
            using var xmlReader = XmlReader.Create(stream, new XmlReaderSettings { IgnoreComments = true });
            var doc = new XmlDocument();
            doc.Load(xmlReader);
            return doc;
        }

        private static WidgetPrefab? MovieRequested(string movie) => movie switch
        {
            "ModOptionsView_MCM" => PrefabInjector.InjectDocumentAndCreate("ModOptionsView_v1", Load("MCM.UI.GUI.v1.Prefabs.ModOptionsView.xml")),
            "EditValueView_MCM" => PrefabInjector.InjectDocumentAndCreate("EditValueView_v1", Load("MCM.UI.GUI.v1.Prefabs.EditValueView.xml")),
            "OptionsWithModOptionsView_MCM" => PrefabInjector.InjectDocumentAndCreate("OptionsWithModOptionsView_v1", Load("MCM.UI.GUI.v1.Prefabs.OptionsWithModOptionsView.xml")),
            _ => null
        };

        public Pre154ResourceInjector()
        {
            var harmony = new Harmony("bannerlord.mcm.resourcehandler.pre154");
            BrushFactoryPatch.Patch(harmony);
            GauntletMoviePatch.Patch(harmony, MovieRequested);

            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v1.Brushes.ButtonBrushes.xml"));
            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v1.Brushes.DividerBrushes.xml"));
            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v1.Brushes.ExpandIndicator.xml"));
            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v1.Brushes.SettingsBrush.xml"));
            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v1.Brushes.ResetButtonBrush.xml"));
            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v1.Brushes.SettingsValueDisplayBrush.xml"));
            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v1.Brushes.TextBrushes.xml"));

            PrefabInjector.InjectDocumentAndCreate("DropdownWithHorizontalControlCheckboxView_v1", Load("MCM.UI.GUI.v1.Prefabs.DropdownWithHorizontalControl.Checkbox.xml"));
            PrefabInjector.InjectDocumentAndCreate("ModOptionsPageView_v1", Load("MCM.UI.GUI.v1.Prefabs.ModOptionsPageView.xml"));
            PrefabInjector.InjectDocumentAndCreate("SettingsItemView_v1", Load("MCM.UI.GUI.v1.Prefabs.SettingsItemView.xml"));
            PrefabInjector.InjectDocumentAndCreate("SettingsPropertyGroupView_v1", Load("MCM.UI.GUI.v1.Prefabs.SettingsPropertyGroupView.xml"));
            PrefabInjector.InjectDocumentAndCreate("SettingsPropertyView_v1", Load("MCM.UI.GUI.v1.Prefabs.SettingsPropertyView.xml"));
            PrefabInjector.InjectDocumentAndCreate("SettingsView_v1", Load("MCM.UI.GUI.v1.Prefabs.SettingsView.xml"));

            WidgetInjector.InjectWidget(typeof(EditValueTextWidget));
            WidgetInfo.ReLoad();
        }
    }
}