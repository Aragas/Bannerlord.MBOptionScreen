using System;

namespace MBOptionScreen.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    internal class SettingPropertyDropdownAttribute : BaseSettingPropertyAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public int SelectedIndex { get; } = 0;

        public SettingPropertyDropdownAttribute(string displayName, int selectedIndex = 0, int order = -1, bool requireRestart = true, string hintText = "")
            : base(displayName, order, requireRestart, hintText)
        {
            SelectedIndex = selectedIndex;
        }
    }
}