using MCM.Abstractions.Functionality;
using MCM.UI.GUI.Views;

using TaleWorlds.GauntletUI;

namespace MCM.UI.ResourceInjection.Loaders
{
    /// <summary>
    /// Loads the embed .xml files from the library
    /// </summary>
    internal static class WidgetLoader
    {
        public static void Inject(BaseResourceHandler resourceInjector)
        {
            resourceInjector.InjectWidget(typeof(EditValueTextWidget_v3));
            resourceInjector.InjectWidget(typeof(HoverRichTextWidget_v3));
            WidgetInfo.ReLoad();
        }
    }
}