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
    sealed class SettingPropertyFloatingIntegerAttribute : BaseSettingPropertyAttribute,
        IPropertyDefinitionWithMinMax,
        IPropertyDefinitionWithFormat,
        IPropertyDefinitionWithCustomFormatter
    {
        /// <inheritdoc/>
        public decimal MinValue { get; }
        /// <inheritdoc/>
        public decimal MaxValue { get; }
        /// <inheritdoc/>
        public string ValueFormat { get; }
        /// <inheritdoc/>
        public Type? CustomFormatter { get; set; }

        public SettingPropertyFloatingIntegerAttribute(string displayName, float minValue, float maxValue, string valueFormat = "0.00") : base(displayName)
        {
            MinValue = (decimal) minValue;
            MaxValue = (decimal) maxValue;
            ValueFormat = valueFormat;
        }
    }
}