using MCM.Abstractions.Settings.Definitions;

using System.Reflection;

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
        string SettingsId { get; }
        PropertyInfo Property { get; }

        SettingType SettingType { get; }
    }
}