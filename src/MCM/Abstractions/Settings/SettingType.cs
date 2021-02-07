using System.Diagnostics.CodeAnalysis;

namespace MCM.Abstractions.Settings
{
    public enum SettingType
    {
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        NONE = -1,
        Bool,
        Int,
        Float,
        String,
        Dropdown
    }
}