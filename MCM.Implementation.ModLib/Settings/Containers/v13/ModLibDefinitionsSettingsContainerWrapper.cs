using MCM.Abstractions.Settings.Containers;

namespace MCM.Implementation.ModLib.Settings.Containers.v13
{
    /// <summary>
    /// For DI
    /// </summary>
    public sealed class ModLibDefinitionsSettingsContainerWrapper : BaseSettingsContainerWrapper, IModLibDefinitionsSettingsContainer
    {
        public ModLibDefinitionsSettingsContainerWrapper(object @object) : base(@object) { }
    }
}