using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.FluentBuilder.Models;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.Definitions.Wrapper;

using System;
using System.Collections.Generic;

namespace MCM.Implementation.FluentBuilder.Models
{
    internal sealed class DefaultSettingsPropertyFloatingIntegerBuilder :
        BaseDefaultSettingsPropertyBuilder<ISettingsPropertyFloatingIntegerBuilder>,
        ISettingsPropertyFloatingIntegerBuilder,
        IPropertyDefinitionWithMinMax,
        IPropertyDefinitionWithFormat,
        IPropertyDefinitionWithActionFormat
    {
        /// <inheritdoc/>
        public decimal MinValue { get; }
        /// <inheritdoc/>
        public decimal MaxValue { get; }
        /// <inheritdoc/>
        public string ValueFormat { get; private set; } = string.Empty;
        /// <inheritdoc/>
        public Func<object, string>? ValueFormatFunc { get; private set; }

        internal DefaultSettingsPropertyFloatingIntegerBuilder(string id, string name, float minValue, float maxValue, IRef @ref)
            : base(id, name, @ref)
        {
            SettingsPropertyBuilder = this;
            MinValue = (decimal) minValue;
            MaxValue = (decimal) maxValue;
        }

        /// <inheritdoc/>
        public ISettingsPropertyBuilder AddValueFormat(string value)
        {
            if (ValueFormatFunc != null)
                throw new InvalidOperationException("AddActionValueFormat was already called!");

            ValueFormat = value;
            return SettingsPropertyBuilder;
        }
        /// <inheritdoc/>
        public ISettingsPropertyBuilder AddActionValueFormat(Func<float, string> value)
        {
            if (!string.IsNullOrEmpty(ValueFormat))
                throw new InvalidOperationException("AddValueFormat was already called!");

            ValueFormatFunc = obj => value((int) obj);
            return SettingsPropertyBuilder;
        }

        /// <inheritdoc/>
        public override IEnumerable<IPropertyDefinitionBase> GetDefinitions() => new IPropertyDefinitionBase[]
        {
            new PropertyDefinitionWithMinMaxWrapper(this),
            new PropertyDefinitionWithFormatWrapper(this),
            new PropertyDefinitionWithActionFormatWrapper(this),
            new PropertyDefinitionWithCustomFormatterWrapper(this),
            new PropertyDefinitionWithIdWrapper(this),
        };
    }
}