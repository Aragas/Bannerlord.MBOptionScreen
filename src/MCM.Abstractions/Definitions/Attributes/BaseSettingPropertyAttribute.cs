using MCM.Common;

using System;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable once CheckNamespace
namespace MCM.Abstractions.Attributes
{
    [SuppressMessage("Design", "RCS1203:Use AttributeUsageAttribute.", Justification = "Implemented in the derived attributes.")]
    public abstract class BaseSettingPropertyAttribute : Attribute, IPropertyDefinitionBase
    {
        /// <inheritdoc/>
        public string DisplayName { get; }
        /// <inheritdoc/>
        public int Order { get; set; }
        /// <inheritdoc/>
        public bool RequireRestart { get; set; }
        /// <inheritdoc/>
        public string HintText { get; set; }

        protected BaseSettingPropertyAttribute(string displayName, int order = -1, bool requireRestart = true, string hintText = "")
        {
            DisplayName = displayName;
            Order = order;
            RequireRestart = requireRestart;
            HintText = hintText;
        }
    }
}