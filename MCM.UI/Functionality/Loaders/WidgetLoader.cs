using MCM.Abstractions.Functionality;
using MCM.UI.GUI.Views;

using TaleWorlds.GauntletUI;

namespace MCM.UI.Functionality.Loaders
{
    /// <summary>
    /// Loads the embed .xml files from the library
    /// </summary>
    internal static class WidgetLoader
    {
        public static void Inject(BaseResourceHandler resourceInjector)
        {
            resourceInjector.InjectWidget(typeof(EditValueTextWidget));
            WidgetInfo.ReLoad();
        }
    }
}