﻿using MCM.Abstractions;
using MCM.Abstractions.FluentBuilder.Models;
using MCM.Abstractions.Wrapper;
using MCM.Common;

using System.Collections.Generic;

namespace MCM.Implementation.FluentBuilder.Models
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
    internal sealed class DefaultSettingsPropertyButtonBuilder :
        BaseDefaultSettingsPropertyBuilder<ISettingsPropertyButtonBuilder>,
        ISettingsPropertyButtonBuilder,
        IPropertyDefinitionButton
    {
        public string Content { get; }

        internal DefaultSettingsPropertyButtonBuilder(string id, string name, IRef @ref, string content)
            : base(id, name, @ref)
        {
            SettingsPropertyBuilder = this;
            Content = content;
        }

        /// <inheritdoc/>
        public override IEnumerable<IPropertyDefinitionBase> GetDefinitions() => new IPropertyDefinitionBase[]
        {
            new PropertyDefinitionButtonWrapper(this),
            new PropertyDefinitionWithIdWrapper(this),
        };
    }
}