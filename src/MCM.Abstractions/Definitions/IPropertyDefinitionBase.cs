namespace MCM.Abstractions
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface IPropertyDefinitionBase
    {
        /// <summary>
        /// The display name of the setting in the settings menu.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Require restart of the game if the value is changed.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Require restart of the game if the value is changed.
        /// </summary>
        bool RequireRestart { get; }

        /// <summary>
        /// The hint text that is displayed at the bottom of the screen when the user hovers over the setting in the settings menu.
        /// </summary>
        string HintText { get; }
    }
}