using MBOptionScreen.Settings;

using System;

namespace MBOptionScreen.Attributes.v2
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class SettingPropertyDropdownAttribute : BaseSettingPropertyAttribute, IPropertyDefinitionDropdown
    {
        public SettingPropertyDropdownAttribute(string displayName) : base(displayName)
        {

        }
    }
}