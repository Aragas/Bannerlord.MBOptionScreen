using Bannerlord.BUTR.Shared.Helpers;

using MCM.Abstractions.Settings.Definitions;

using System;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable once CheckNamespace
namespace MCM.Abstractions.Attributes
{
    [SuppressMessage("Design", "RCS1203:Use AttributeUsageAttribute.", Justification = "Implemented in the derived attributes.")]
    public abstract class BaseSettingPropertyAttribute : Attribute, IPropertyDefinitionBase
    {
        private string _hintText = string.Empty;

        /// <inheritdoc/>
        public string DisplayName { get; }
        /// <inheritdoc/>
        public int Order { get; set; }
        /// <inheritdoc/>
        public bool RequireRestart { get; set; }
        /// <inheritdoc/>
        public string HintText { get => _hintText; set => _hintText = TextObjectHelper.Create(value)?.ToString() ?? "ERROR"; }

        protected BaseSettingPropertyAttribute(string displayName, int order = -1, bool requireRestart = true, string hintText = "")
        {
            DisplayName = TextObjectHelper.Create(displayName)?.ToString() ?? "ERROR";
            Order = order;
            RequireRestart = requireRestart;
            HintText = hintText;
        }
    }
}