using MCM.Abstractions.FluentBuilder.Models;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.Definitions.Wrapper;

using System.Collections.Generic;

namespace MCM.Abstractions.FluentBuilder.Implementation.Models
{
    public class DefaultSettingsPropertyDropdownBuilder :
        BaseDefaultSettingsPropertyBuilder<ISettingsPropertyDropdownBuilder>,
        ISettingsPropertyDropdownBuilder,
        IPropertyDefinitionDropdown
    {
        public int SelectedIndex { get; }

        internal DefaultSettingsPropertyDropdownBuilder(string name, int selectedIndex, IRef @ref)
            : base(name, @ref)
        {
            SettingsPropertyBuilder = this;
            SelectedIndex = selectedIndex;
        }

        public override IEnumerable<IPropertyDefinitionBase> GetDefinitions() => new IPropertyDefinitionBase[]
        {
            new PropertyDefinitionDropdownWrapper(this), 
        };
    }
}