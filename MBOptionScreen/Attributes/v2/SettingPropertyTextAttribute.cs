using MBOptionScreen.Settings;

using System;

namespace MBOptionScreen.Attributes.v2
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class SettingPropertyTextAttribute : BaseSettingPropertyAttribute, IPropertyDefinitionText
    {
        public SettingPropertyTextAttribute(string displayName, int order = -1, bool requireRestart = true, string hintText = "")
            : base(displayName, order, requireRestart, hintText)
        {

        }
    }
}