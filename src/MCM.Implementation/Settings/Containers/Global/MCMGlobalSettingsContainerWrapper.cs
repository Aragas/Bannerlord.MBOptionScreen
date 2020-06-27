using MCM.Abstractions.Settings.Containers.Global;

namespace MCM.Implementation.Settings.Containers.Global
{
    /// <summary>
    /// For DI
    /// </summary>
    public sealed class MCMGlobalSettingsContainerWrapper : BaseGlobalSettingsContainerWrapper, IMCMGlobalSettingsContainer
    {
        public MCMGlobalSettingsContainerWrapper(object @object) : base(@object) { }
    }
}