using HarmonyLib;

using MCM.UI.Exceptions;
using MCM.UI.GUI.Views;
using MCM.UI.Patches;

using System.Xml;

using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.PrefabSystem;

namespace MCM.UI.Functionality.Injectors
{
    internal class ResourceInjectorPre154 : IResourceInjector
    {
        private static XmlDocument Load(string embedPath)
        {
            using var stream = typeof(ResourceInjectorPre154).Assembly.GetManifestResourceStream(embedPath);
            if (stream is null) throw new MCMUIEmbedResourceNotFoundException($"Could not find embed resource '{embedPath}'!");
            using var xmlReader = XmlReader.Create(stream, new XmlReaderSettings { IgnoreComments = true });
            var doc = new XmlDocument();
            doc.Load(xmlReader);
            return doc;
        }

        private static WidgetPrefab? MovieRequested(string movie) => movie switch
        {
            "ModOptionsView_MCM" => PrefabInjector.CreateAndInject("ModOptionsView_v1", Load("MCM.UI.GUI.v1.Prefabs.ModOptionsView.xml")),
            "EditValueView_MCM" => PrefabInjector.CreateAndInject("EditValueView_v1", Load("MCM.UI.GUI.v1.Prefabs.EditValueView.xml")),
            "OptionsWithModOptionsView_MCM" => PrefabInjector.CreateAndInject("OptionsWithModOptionsView_v1", Load("MCM.UI.GUI.v1.Prefabs.OptionsWithModOptionsView.xml")),
            _ => null
        };

        public void Inject()
        {
            var harmony = new Harmony("bannerlord.mcm.resourcehandler.pre154");
            BrushFactoryPre154Patch.Patch(harmony);
            GauntletMoviePatch.Patch(harmony, MovieRequested);
            WidgetPrefabPatch.Patch(harmony);

            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v1.Brushes.ButtonBrushes.xml"));
            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v1.Brushes.DividerBrushes.xml"));
            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v1.Brushes.ExpandIndicator.xml"));
            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v1.Brushes.SettingsBrush.xml"));
            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v1.Brushes.ResetButtonBrush.xml"));
            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v1.Brushes.SettingsValueDisplayBrush.xml"));
            BrushInjector.InjectDocument(Load("MCM.UI.GUI.v1.Brushes.TextBrushes.xml"));

            PrefabInjector.CreateAndInject("DropdownWithHorizontalControlCheckboxView_v1", Load("MCM.UI.GUI.v1.Prefabs.DropdownWithHorizontalControl.Checkbox.xml"));
            PrefabInjector.CreateAndInject("ModOptionsPageView_v1", Load("MCM.UI.GUI.v1.Prefabs.ModOptionsPageView.xml"));
            PrefabInjector.CreateAndInject("SettingsItemView_v1", Load("MCM.UI.GUI.v1.Prefabs.SettingsItemView.xml"));
            PrefabInjector.CreateAndInject("SettingsPropertyGroupView_v1", Load("MCM.UI.GUI.v1.Prefabs.SettingsPropertyGroupView.xml"));
            PrefabInjector.CreateAndInject("SettingsPropertyView_v1", Load("MCM.UI.GUI.v1.Prefabs.SettingsPropertyView.xml"));
            PrefabInjector.CreateAndInject("SettingsView_v1", Load("MCM.UI.GUI.v1.Prefabs.SettingsView.xml"));

            WidgetInjector.InjectWidget(typeof(EditValueTextWidget));
            WidgetInfo.ReLoad();
        }
    }
}