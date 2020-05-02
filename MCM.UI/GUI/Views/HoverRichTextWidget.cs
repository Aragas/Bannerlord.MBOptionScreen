using System;

using TaleWorlds.GauntletUI;
using TaleWorlds.Library;

namespace MCM.UI.GUI.Views
{
    internal class HoverRichTextWidget_v3 : RichTextWidget
    {
        [DataSourceProperty]
        public Action HoverBegin { get; set; } = null!;
        [DataSourceProperty]
        public Action HoverEnd { get; set; } = null!;

        public HoverRichTextWidget_v3(UIContext context) : base(context)
        {
        }

        protected override void OnHoverBegin()
        {
            base.OnHoverBegin();
            HoverBegin?.Invoke();
        }

        protected override void OnHoverEnd()
        {
            base.OnHoverEnd();
            HoverEnd?.Invoke();
        }
    }
}
