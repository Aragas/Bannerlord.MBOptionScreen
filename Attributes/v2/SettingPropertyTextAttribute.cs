using System;

namespace MBOptionScreen.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    internal class SettingPropertyTextAttribute : BaseSettingPropertyAttribute
    {
        public SettingPropertyTextAttribute(string displayName, int order = -1, bool requireRestart = true, string hintText = "")
            : base(displayName, order, requireRestart, hintText)
        {

        }
    }
}