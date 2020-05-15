using MCM.Abstractions.FluentBuilder.Models;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.Definitions.Wrapper;

using System.Collections.Generic;

namespace MCM.Abstractions.FluentBuilder.Implementation.Models
{
    public class DefaultSettingsPropertyTextBuilder :
        BaseDefaultSettingsPropertyBuilder<ISettingsPropertyTextBuilder>,
        ISettingsPropertyTextBuilder,
        IPropertyDefinitionText
    {
        internal DefaultSettingsPropertyTextBuilder(string name, IRef @ref)
            : base(name, @ref)
        {
            SettingsPropertyBuilder = this;
        }

        public override IEnumerable<IPropertyDefinitionBase> GetDefinitions() => new IPropertyDefinitionBase[]
        {
            new PropertyDefinitionTextWrapper(this), 
        };
    }
}