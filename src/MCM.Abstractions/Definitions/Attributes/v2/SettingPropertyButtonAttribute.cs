using MCM.Abstractions.Settings.Definitions;

using System;

// ReSharper disable once CheckNamespace
namespace MCM.Abstractions.Attributes.v2
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class SettingPropertyButtonAttribute : BaseSettingPropertyAttribute,
        IPropertyDefinitionButton
    {
        public string Content { get; set; } = string.Empty;

        public SettingPropertyButtonAttribute(string displayName, int order = -1, bool requireRestart = true, string hintText = "")
            : base(displayName, order, requireRestart, hintText) { }
    }
}