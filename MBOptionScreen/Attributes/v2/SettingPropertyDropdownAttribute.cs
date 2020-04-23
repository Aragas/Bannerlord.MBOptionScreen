using MBOptionScreen.Settings;

using System;

namespace MBOptionScreen.Attributes.v2
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    internal sealed class SettingPropertyDropdownAttribute : BaseSettingPropertyAttribute, IPropertyDefinitionDropdown
    {
        /// <summary>
        ///
        /// </summary>
        public int SelectedIndex { get; } = 0;

        public SettingPropertyDropdownAttribute(string displayName, int selectedIndex) : base(displayName)
        {
            SelectedIndex = selectedIndex;
        }
    }
}