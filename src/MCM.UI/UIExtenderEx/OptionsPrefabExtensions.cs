using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.Prefabs;

using System.Xml;

namespace MCM.UI.UIExtenderEx
{
    [PrefabExtension("Options", "descendant::OptionsScreenWidget[@Id='Options']/Children/Standard.TopPanel/Children/ListPanel/Children")]
    public sealed class OptionsPrefabExtension1 : PrefabExtensionInsertPatch
    {
        public override string Id => "Options1";
        public override int Position => 3;
        private XmlDocument XmlDocument { get; } = new XmlDocument();

        public OptionsPrefabExtension1()
        {
            XmlDocument.LoadXml("<OptionsTabToggle DataSource=\"{ModOptions}\" PositionYOffset=\"2\" Parameter.ButtonBrush=\"Header.Tab.Center\" Parameter.TabName=\"ModOptionsPage\" />");
        }

        public override XmlDocument GetPrefabExtension() => XmlDocument;
    }

    [PrefabExtension("Options", "descendant::TabControl[@Id='TabControl']/Children")]
    internal sealed class OptionsPrefabExtension2 : PrefabExtensionInsertPatch
    {
        public override string Id => "Options2";
        public override int Position => 3;
        private XmlDocument XmlDocument { get; } = new XmlDocument();

        public OptionsPrefabExtension2()
        {
            XmlDocument.LoadXml("<ModOptionsPageView_MCMv3 Id=\"ModOptionsPage\" DataSource=\"{ModOptions}\" />");
        }

        public override XmlDocument GetPrefabExtension() => XmlDocument;
    }

    [PrefabExtension("Options", "descendant::OptionsScreenWidget[@Id='Options']/Children/ListPanel[@Id='MainSectionListPanel']/Children/Widget[@Id='DescriptionsRightPanel']")]
    public sealed class OptionsPrefabExtension3 : PrefabExtensionReplacePatch
    {
        public override string Id => "Options3";
        private XmlDocument XmlDocument { get; } = new XmlDocument();

        public OptionsPrefabExtension3()
        {
            XmlDocument.LoadXml(@"
<Widget Id=""DescriptionsRightPanel"" WidthSizePolicy=""Fixed"" HeightSizePolicy=""StretchToParent"" SuggestedWidth=""@DescriptionWidth"">
  <Children>
    <ListPanel Id=""DescriptionsListPanel"" WidthSizePolicy=""StretchToParent"" HeightSizePolicy=""CoverChildren"" MarginLeft=""40"" MarginTop=""65"" LayoutImp.LayoutMethod=""VerticalBottomToTop"">
      <Children>
        <RichTextWidget Id=""CurrentOptionNameWidget"" WidthSizePolicy=""StretchToParent"" HeightSizePolicy=""CoverChildren"" Brush=""SPOptions.Description.Title.Text"" Text="" "" />
        <RichTextWidget Id=""CurrentOptionDescriptionWidget"" WidthSizePolicy=""StretchToParent"" HeightSizePolicy=""CoverChildren"" MarginTop=""25"" Brush=""SPOptions.Description.Text"" Text="" "" />
        <Widget Id=""CurrentOptionImageWidget"" WidthSizePolicy=""Fixed"" HeightSizePolicy=""Fixed"" SuggestedWidth=""576"" SuggestedHeight=""324"" MarginTop=""35"" />
      </Children>
    </ListPanel>
  </Children>
</Widget>
");
        }

        public override XmlDocument GetPrefabExtension() => XmlDocument;
    }
}