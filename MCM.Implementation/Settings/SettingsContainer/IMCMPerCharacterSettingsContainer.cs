using MCM.Abstractions;
using MCM.Abstractions.Settings.SettingsContainer;

namespace MCM.Implementation.Settings.SettingsContainer
{
    /// <summary>
    /// So it can be overriden by an external library
    /// </summary>
    public interface IMCMPerCharacterSettingsContainer : IPerCharacterSettingsContainer, IDependency
    {

    }
}