extern alias v4;

using v4::MCM.Abstractions.Settings.Base.Global;

namespace MCM.Implementation.MBO.Settings.Base
{
    public abstract class BaseMBOGlobalSettingsWrapper : BaseGlobalSettingsWrapper
    {
        protected BaseMBOGlobalSettingsWrapper(object @object) : base(@object) { }
    }
}