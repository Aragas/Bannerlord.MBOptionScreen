using MCM.Abstractions;
using MCM.Abstractions.Settings.SettingsContainer;

namespace MCM.Implementation.ModLib.Settings.SettingsContainer
{
    public sealed class ModLibDefinitionsSettingsContainerWrapper : SettingsContainerWrapper, IModLibDefinitionsSettingsContainer, IWrapper
    {
        public ModLibDefinitionsSettingsContainerWrapper(object @object) : base(@object) { }
    }
}