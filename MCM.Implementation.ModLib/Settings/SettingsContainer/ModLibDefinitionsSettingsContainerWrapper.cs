using MCM.Abstractions;
using MCM.Abstractions.Settings.SettingsContainer;

namespace MCM.Implementation.ModLib.Settings.SettingsContainer
{
    /// <summary>
    /// For DI
    /// </summary>
    public sealed class ModLibDefinitionsSettingsContainerWrapper : BaseSettingsContainerWrapper, IModLibDefinitionsSettingsContainer, IWrapper
    {
        public ModLibDefinitionsSettingsContainerWrapper(object @object) : base(@object) { }
    }
}