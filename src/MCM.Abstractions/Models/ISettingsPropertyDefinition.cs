using MCM.Common;

namespace MCM.Abstractions
{
    public interface ISettingsPropertyDefinition :
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