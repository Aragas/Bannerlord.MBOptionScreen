using System;

namespace MBOptionScreen.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    internal class SettingPropertyBoolAttribute : BaseSettingPropertyAttribute
    {
        public SettingPropertyBoolAttribute(string displayName, int order = -1, bool requireRestart = true, string hintText = "")
            : base(displayName, order, requireRestart, hintText)
        {

        }
    }
}