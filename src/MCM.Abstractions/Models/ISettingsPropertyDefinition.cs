using MCM.Common;

namespace MCM.Abstractions
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface ISettingsPropertyDefinition :
        IPropertyDefinitionBase,
        IPropertyDefinitionBool,
        IPropertyDefinitionDropdown,
        IPropertyDefinitionWithMinMax,
        IPropertyDefinitionWithEditableMinMax,
        IPropertyDefinitionWithFormat,
        IPropertyDefinitionWithCustomFormatter,
        IPropertyDefinitionWithId,
        IPropertyDefinitionText,
        IPropertyDefinitionGroupToggle,
        IPropertyGroupDefinition,
        IPropertyDefinitionButton
    {
        IRef PropertyReference { get; }

        SettingType SettingType { get; }

        SettingsPropertyDefinition Clone(bool keepRefs = true);
    }
}