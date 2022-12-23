using System.Diagnostics.CodeAnalysis;

namespace MCM.Abstractions
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    enum SettingType
    {
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "For ReSharper")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        NONE = -1,
        Bool,
        Int,
        Float,
        String,
        Dropdown,
        Button
    }
}