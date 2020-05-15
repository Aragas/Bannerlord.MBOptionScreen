using MCM.Abstractions.FluentBuilder.Models;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.Definitions.Wrapper;

using System.Collections.Generic;

namespace MCM.Abstractions.FluentBuilder.Implementation.Models
{
    public class DefaultSettingsPropertyFloatingIntegerBuilder :
        BaseDefaultSettingsPropertyBuilder<ISettingsPropertyFloatingIntegerBuilder>,
        ISettingsPropertyFloatingIntegerBuilder,
        IPropertyDefinitionWithMinMax,
        IPropertyDefinitionWithFormat
    {
        public decimal MinValue { get; }
        public decimal MaxValue { get; }
        public string ValueFormat { get; private set; } = "";

        internal DefaultSettingsPropertyFloatingIntegerBuilder(string name, float minValue, float maxValue, IRef @ref)
            : base(name, @ref)
        {
            SettingsPropertyBuilder = this;
            MinValue = (decimal) minValue;
            MaxValue = (decimal) maxValue;
        }

        public ISettingsPropertyBuilder AddValueFormat(string value)
        {
            ValueFormat = value;
            return SettingsPropertyBuilder;
        }

        public override IEnumerable<IPropertyDefinitionBase> GetDefinitions() => new IPropertyDefinitionBase[]
        {
            new PropertyDefinitionWithMinMaxWrapper(this),
            new PropertyDefinitionWithFormatWrapper(this),
        };
    }
}