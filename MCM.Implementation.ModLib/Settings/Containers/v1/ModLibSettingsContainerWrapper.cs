using MCM.Abstractions;
using MCM.Abstractions.Settings.Containers;

namespace MCM.Implementation.ModLib.Settings.Containers.v1
{
    /// <summary>
    /// For DI
    /// </summary>
    public sealed class ModLibSettingsContainerWrapper : BaseSettingsContainerWrapper, IModLibSettingsContainer, IWrapper
    {
        public ModLibSettingsContainerWrapper(object @object) : base(@object) { }
    }
}