using System;

// ReSharper disable once CheckNamespace
namespace MCM.Abstractions.Attributes.v2
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    sealed class SettingPropertyTextAttribute : BaseSettingPropertyAttribute,
        IPropertyDefinitionText
    {
        public SettingPropertyTextAttribute(string displayName, int order = -1, bool requireRestart = true, string hintText = "")
            : base(displayName, order, requireRestart, hintText) { }
    }
}