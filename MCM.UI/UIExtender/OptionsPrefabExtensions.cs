#if UIE
using UIExtenderLib.Interface;

namespace MCM.UI.UIExtender
{
    [PrefabExtension("Options", "descendant::OptionsScreenWidget[@Id='Options']/Children/Standard.TopPanel/Children/ListPanel/Children")]
    public class OptionsPrefabExtension1 : PrefabExtensionInsertPatch
    {
        public override int Position => 5;
        public override string Name => "Options1";
    }
    [PrefabExtension("Options", "descendant::TabControl[@Id='TabControl']/Children")]
    public class OptionsPrefabExtension2 : PrefabExtensionInsertPatch
    {
        public override int Position => 5;
        public override string Name => "Options2";
    }
}
#endif