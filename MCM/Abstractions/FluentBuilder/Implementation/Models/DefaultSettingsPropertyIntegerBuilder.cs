using MCM.Abstractions.FluentBuilder.Models;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.Definitions.Wrapper;

using System;
using System.Collections.Generic;

namespace MCM.Abstractions.FluentBuilder.Implementation.Models
{
    public class DefaultSettingsPropertyIntegerBuilder :
        BaseDefaultSettingsPropertyBuilder<ISettingsPropertyIntegerBuilder>,
        ISettingsPropertyIntegerBuilder,
        IPropertyDefinitionWithMinMax,
        IPropertyDefinitionWithFormat,
        IPropertyDefinitionWithActionFormat
    {
        public decimal MinValue { get; }
        public decimal MaxValue { get; }
        public string ValueFormat { get; private set; } = "";
        public Func<object, string> ValueFormatFunc { get; private set; }
        
        internal DefaultSettingsPropertyIntegerBuilder(string name, int minValue, int maxValue, IRef @ref)
            : base(name, @ref)
        {
            SettingsPropertyBuilder = this;
            MinValue = (decimal) minValue;
            MaxValue = (decimal) maxValue;
        }

        public ISettingsPropertyBuilder AddValueFormat(string value)
        {
            if (ValueFormatFunc != null)
                throw new InvalidOperationException("AddActionValueFormat was already called!");

            ValueFormat = value;
            return SettingsPropertyBuilder;
        }
        public ISettingsPropertyBuilder AddActionValueFormat(Func<int, string> value)
        {
            if (ValueFormat != "")
                throw new InvalidOperationException("AddValueFormat was already called!");

            ValueFormatFunc = obj => value((int) obj);
            return SettingsPropertyBuilder;
        }

        public override IEnumerable<IPropertyDefinitionBase> GetDefinitions() => new IPropertyDefinitionBase[]
        {
            new PropertyDefinitionWithMinMaxWrapper(this),
            new PropertyDefinitionWithFormatWrapper(this), 
            new PropertyDefinitionWithActionFormatWrapper(this),
        };
    }
}