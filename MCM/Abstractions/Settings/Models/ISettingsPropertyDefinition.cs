using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Definitions;

namespace MCM.Abstractions.Settings.Models
{
    public interface ISettingsPropertyDefinition :
        IPropertyDefinitionBase,
        IPropertyDefinitionBool,
        IPropertyDefinitionDropdown,
        IPropertyDefinitionWithMinMax,
        IPropertyDefinitionWithEditableMinMax,
        IPropertyDefinitionWithFormat,
        IPropertyDefinitionText,
        IPropertyGroupDefinition
    {
        IRef PropertyReference { get; }

        SettingType SettingType { get; }
    }
}