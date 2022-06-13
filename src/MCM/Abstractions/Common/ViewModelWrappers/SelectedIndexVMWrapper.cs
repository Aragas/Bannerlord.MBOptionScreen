using MCM.Abstractions.Common.Wrappers;

namespace MCM.Abstractions.Common.ViewModelWrappers
{
    public class SelectedIndexVMWrapper : ViewModelWrapper
    {
        public int SelectedIndex
        {
            get => new SelectedIndexWrapper(Object).SelectedIndex;
            set
            {
                var selectedIndexWrapper = new SelectedIndexWrapper(Object);
                selectedIndexWrapper.SelectedIndex = value;
            }
        }

        public SelectedIndexVMWrapper(object @object) : base(@object) { }
    }
}