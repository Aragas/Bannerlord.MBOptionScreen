using MCM.Abstractions.Settings.SettingsContainer;

namespace MCM.Implementation.Settings.SettingsContainer
{
    /// <summary>
    /// For DI
    /// </summary>
    public sealed class MCMGlobalSettingsContainerWrapper : BaseGlobalSettingsContainerWrapper, IMCMGlobalSettingsContainer
    {
        public MCMGlobalSettingsContainerWrapper(object @object) : base(@object) { }
    }
}