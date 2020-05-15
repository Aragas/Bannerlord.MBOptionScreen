using MCM.Abstractions;
using MCM.Abstractions.Settings.SettingsContainer;

namespace MCM.Implementation.ModLib.Settings.SettingsContainer
{
    /// <summary>
    /// For DI
    /// </summary>
    public sealed class ModLibSettingsContainerWrapper : BaseSettingsContainerWrapper, IModLibSettingsContainer, IWrapper
    {
        public ModLibSettingsContainerWrapper(object @object) : base(@object) { }
    }
}