using MCM.Abstractions.Settings.Definitions;

using System;

// ReSharper disable once CheckNamespace
namespace MCM.Abstractions.Attributes.v2
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class SettingPropertyBoolAttribute : BaseSettingPropertyAttribute,
        IPropertyDefinitionBool,
        IPropertyDefinitionGroupToggle
    {
        public bool IsToggle { get; set; }

        public SettingPropertyBoolAttribute(string displayName) : base(displayName) { }
    }
}