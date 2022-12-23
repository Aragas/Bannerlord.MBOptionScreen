using MCM.Abstractions;
using MCM.Abstractions.FluentBuilder.Models;
using MCM.Abstractions.Wrapper;
using MCM.Common;

using System.Collections.Generic;

namespace MCM.Implementation.FluentBuilder.Models
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
    internal sealed class DefaultSettingsPropertyGroupToggleBuilder :
        BaseDefaultSettingsPropertyBuilder<ISettingsPropertyGroupToggleBuilder>,
        ISettingsPropertyGroupToggleBuilder,
        IPropertyDefinitionGroupToggle
    {
        /// <inheritdoc/>
        public bool IsToggle { get; } = true;

        internal DefaultSettingsPropertyGroupToggleBuilder(string id, string name, IRef @ref)
            : base(id, name, @ref)
        {
            SettingsPropertyBuilder = this;
        }

        /// <inheritdoc/>
        public override IEnumerable<IPropertyDefinitionBase> GetDefinitions() => new IPropertyDefinitionBase[]
        {
            new PropertyDefinitionGroupToggleWrapper(this),
            new PropertyDefinitionWithIdWrapper(this),
        };
    }
}