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
        IPropertyDefinitionWithCustomFormatter,
        IPropertyDefinitionText,
        IPropertyGroupDefinition
    {
        // TODO: v4
        //string Id { get; }

        IRef PropertyReference { get; }

        SettingType SettingType { get; }
    }
}