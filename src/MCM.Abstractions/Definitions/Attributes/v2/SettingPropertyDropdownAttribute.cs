using MCM.Abstractions.Settings.Definitions;

using System;

// ReSharper disable once CheckNamespace
namespace MCM.Abstractions.Attributes.v2
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class SettingPropertyDropdownAttribute : BaseSettingPropertyAttribute,
        IPropertyDefinitionDropdown
    {
        /// <inheritdoc/>
        public int SelectedIndex { get; }

        public SettingPropertyDropdownAttribute(string displayName) : base(displayName) { }
    }
}