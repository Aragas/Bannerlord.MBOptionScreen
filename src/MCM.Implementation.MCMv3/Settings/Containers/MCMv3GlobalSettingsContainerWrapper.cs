extern alias v4;

using v4::MCM.Abstractions.Settings.Containers;

namespace MCM.Implementation.MCMv3.Settings.Containers
{
    /// <summary>
    /// For DI
    /// </summary>
    public sealed class MCMv3GlobalSettingsContainerWrapper : BaseSettingsContainerWrapper, IMCMv3GlobalSettingsContainer
    {
        public MCMv3GlobalSettingsContainerWrapper(object @object) : base(@object) { }
    }
}