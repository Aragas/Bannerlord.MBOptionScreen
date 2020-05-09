using System.Reflection;

namespace MCM.Abstractions.Settings.Definitions
{
    public interface ISettingsPropertyDefinition :
        IPropertyDefinitionBase,
        IPropertyDefinitionBool,
        IPropertyDefinitionDropdown,
        IPropertyDefinitionWithMinMax,
        IPropertyDefinitionWithFormat,
        IPropertyDefinitionText,
        IPropertyGroupDefinition
    {
        PropertyInfo Property { get; }
        string SettingsId { get; }
        SettingType SettingType { get; }

        decimal EditableMinValue { get; }
        decimal EditableMaxValue { get; }
    }
}