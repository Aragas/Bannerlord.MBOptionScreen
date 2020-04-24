using System;

using TaleWorlds.GauntletUI;
using TaleWorlds.Library;

namespace MBOptionScreen.GUI.v1b.Views
{
    public class HoverRichTextWidget_v1b : RichTextWidget
    {
        [DataSourceProperty]
        public Action HoverBegin { get; set; } = null!;
        [DataSourceProperty]
        public Action HoverEnd { get; set; } = null!;

        public HoverRichTextWidget_v1b(UIContext context) : base(context)
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
