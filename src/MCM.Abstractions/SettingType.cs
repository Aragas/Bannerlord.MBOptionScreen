using System.Diagnostics.CodeAnalysis;

namespace MCM.Abstractions
{
    /// <summary>
    /// Defines the different types of settings that can be used.
    /// </summary>
    #if !BANNERLORDMCM_PUBLIC
        internal
    #else
        public
    # endif
    enum SettingType
    {
        /// <summary>
        /// A default value indicating no setting type.
        /// </summary>
        NONE = -1,

        /// <summary>
        /// A boolean setting with true/false values.
        /// </summary>
        Bool,

        /// <summary>
        /// An integer number setting.
        /// </summary>
        Int,

        /// <summary>
        /// A floating point number setting. 
        /// </summary>
        Float,

        /// <summary>
        /// A text string setting.
        /// </summary>
        String,

        /// <summary>
        /// A dropdown list setting.
        /// </summary>
        Dropdown,

        /// <summary>
        /// A button that can trigger some action.
        /// </summary>
        Button,
    }
}