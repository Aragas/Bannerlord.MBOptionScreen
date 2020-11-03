using HarmonyLib;

using MCM.UI.Exceptions;
using MCM.UI.GUI.Views;
using MCM.UI.Patches;

using System.Xml;

using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.PrefabSystem;

namespace MCM.UI.Functionality.Injectors
{
    internal class ResourceInjectorPost154 : IResourceInjector
    {
        private static XmlDocument Load(string embedPath)
        {
            using var stream = typeof(ResourceInjectorPost154).Assembly.GetManifestResourceStream(embedPath);
            if (stream is null) throw new MCMUIEmbedResourceNotFoundException($"Could not find embed resource '{embedPath}'!");
            using var xmlReader = XmlReader.Create(stream, new XmlReaderSettings { IgnoreComments = true });
            var doc = new XmlDocument();
            doc.Load(xmlReader);
            return doc;
        }


        private static WidgetPrefab? MovieRequested(string movie) => movie switch
        {
            "ModOptionsView_MCM" => PrefabInjector.Create("ModOptionsView_v2", Load("MCM.UI.GUI.v2.Prefabs.ModOptionsView.xml")),
            "EditValueView_MCM" => PrefabInjector.Create("EditValueView_v2", Load("MCM.UI.GUI.v2.Prefabs.EditValueView.xml")),
            "OptionsWithModOptionsView_MCM" => PrefabInjector.Create("OptionsWithModOptionsView_v2", Load("MCM.UI.GUI.v2.Prefabs.OptionsWithModOptionsView.xml")),
            _ => null
        };
        private static WidgetPrefab? WidgetRequested(string widget) => widget switch
        {
            "DropdownWithHorizontalControlCheckboxView_v2" => PrefabInjector.Create("DropdownWithHorizontalControlCheckboxView_v2", Load("MCM.UI.GUI.v2.Prefabs.DropdownWithHorizontalControl.Checkbox.xml")),
            "ModOptionsPageView_v2" => PrefabInjector.Create("ModOptionsPageView_v2", Load("MCM.UI.GUI.v2.Prefabs.ModOptionsPageView.xml")),
            "SettingsItemView_v2" => PrefabInjector.Create("SettingsItemView_v2", Load("MCM.UI.GUI.v2.Prefabs.SettingsItemView.xml")),
            "SettingsPropertyGroupView_v2" => PrefabInjector.Create("SettingsPropertyGroupView_v2", Load("MCM.UI.GUI.v2.Prefabs.SettingsPropertyGroupView.xml")),
            "SettingsPropertyView_v2" => PrefabInjector.Create("SettingsPropertyView_v2", Load("MCM.UI.GUI.v2.Prefabs.SettingsPropertyView.xml")),
            "SettingsView_v2" => PrefabInjector.Create("SettingsView_v2", Load("MCM.UI.GUI.v2.Prefabs.SettingsView.xml")),
            _ => null
        };

        public void Inject()
        {
            var harmony = new Harmony("bannerlord.mcm.resourcehandler.post154");
            GauntletMoviePatch.Patch(harmony, MovieRequested);
            WidgetPrefabPatch.Patch(harmony);
            WidgetFactoryPost154Patch.Patch(harmony, WidgetRequested);

            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v2.Brushes.ButtonBrushes.xml"));
            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v2.Brushes.DividerBrushes.xml"));
            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v2.Brushes.ExpandIndicator.xml"));
            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v2.Brushes.SettingsBrush.xml"));
            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v2.Brushes.ResetButtonBrush.xml"));
            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v2.Brushes.SettingsValueDisplayBrush.xml"));
            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v2.Brushes.TextBrushes.xml"));

            PrefabInjector.Register("DropdownWithHorizontalControlCheckboxView_v2");
            PrefabInjector.Register("ModOptionsPageView_v2");
            PrefabInjector.Register("SettingsItemView_v2");
            PrefabInjector.Register("SettingsPropertyGroupView_v2");
            PrefabInjector.Register("SettingsPropertyView_v2");
            PrefabInjector.Register("SettingsView_v2");

            WidgetInjector.InjectWidget(typeof(EditValueTextWidget));
            WidgetInfo.ReLoad();
        }
    }
}