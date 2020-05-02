using MCM.Abstractions.ResourceInjection;
using MCM.UI.ResourceInjection;

using TaleWorlds.MountAndBlade;

namespace MCM.UI
{
    public sealed class SubModuleV300 : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            BrushLoader.Inject(BaseResourceInjector.Instance);
            PrefabsLoader.Inject(BaseResourceInjector.Instance);
            WidgetLoader.Inject(BaseResourceInjector.Instance);
        }
    }
}