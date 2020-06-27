using MCM.Abstractions;
using MCM.Abstractions.Settings.Containers.PerCharacter;

namespace MCM.Implementation.Settings.Containers.PerCharacter
{
    /// <summary>
    /// So it can be overriden by an external library
    /// </summary>
    public interface IMCMPerCharacterSettingsContainer : IPerCharacterSettingsContainer, IDependency
    {

    }
}