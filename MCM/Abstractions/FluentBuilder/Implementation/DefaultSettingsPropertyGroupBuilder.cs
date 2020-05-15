using MCM.Abstractions.FluentBuilder.Implementation.Models;
using MCM.Abstractions.FluentBuilder.Models;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.Definitions.Wrapper;

using System;
using System.Collections.Generic;

namespace MCM.Abstractions.FluentBuilder.Implementation
{
    public class DefaultSettingsPropertyGroupBuilder :
        ISettingsPropertyGroupBuilder,
        IPropertyGroupDefinition
    {
        public Dictionary<string, ISettingsPropertyBuilder> Properties { get; } = new Dictionary<string, ISettingsPropertyBuilder>();

        public string GroupName { get; }
        public bool IsMainToggle { get; private set; }
        public int GroupOrder { get; private set; }

        public DefaultSettingsPropertyGroupBuilder(string name)
        {
            GroupName = name;
        }

        public ISettingsPropertyGroupBuilder SetIsMainToggle(bool value) { IsMainToggle = value; return this; }
        public ISettingsPropertyGroupBuilder SetGroupOrder(int value) { GroupOrder = value; return this; }

        public ISettingsPropertyGroupBuilder AddBool(string name, IRef @ref, Action<ISettingsPropertyBoolBuilder>? builder)
        {
            if (!Properties.ContainsKey(name))
                Properties[name] = new DefaultSettingsPropertyBoolBuilder(name, @ref);
            builder?.Invoke((ISettingsPropertyBoolBuilder) Properties[name]);
            return this;
        }
        public ISettingsPropertyGroupBuilder AddDropdown(string name, int selectedIndex, IRef @ref, Action<ISettingsPropertyDropdownBuilder>? builder)
        {
            if (!Properties.ContainsKey(name))
                Properties[name] = new DefaultSettingsPropertyDropdownBuilder(name, selectedIndex, @ref);
            builder?.Invoke((ISettingsPropertyDropdownBuilder) Properties[name]);
            return this;
        }
        public ISettingsPropertyGroupBuilder AddInteger(string name, int minValue, int maxValue, IRef @ref, Action<ISettingsPropertyIntegerBuilder>? builder)
        {
            if (!Properties.ContainsKey(name))
                Properties[name] = new DefaultSettingsPropertyIntegerBuilder(name, minValue, maxValue, @ref);
            builder?.Invoke((ISettingsPropertyIntegerBuilder) Properties[name]);
            return this;
        }
        public ISettingsPropertyGroupBuilder AddFloatingInteger(string name, float minValue, float maxValue, IRef @ref, Action<ISettingsPropertyFloatingIntegerBuilder>? builder)
        {
            if (!Properties.ContainsKey(name))
                Properties[name] = new DefaultSettingsPropertyFloatingIntegerBuilder(name, minValue, maxValue, @ref);
            builder?.Invoke((ISettingsPropertyFloatingIntegerBuilder) Properties[name]);
            return this;
        }
        public ISettingsPropertyGroupBuilder AddText(string name, IRef @ref, Action<ISettingsPropertyTextBuilder>? builder)
        {
            if (!Properties.ContainsKey(name))
                Properties[name] = new DefaultSettingsPropertyTextBuilder(name, @ref);
            builder?.Invoke((ISettingsPropertyTextBuilder) Properties[name]);
            return this;
        }
        public ISettingsPropertyGroupBuilder AddCustom<TSettingsPropertyBuilder>(ISettingsPropertyBuilder<TSettingsPropertyBuilder> builder) where TSettingsPropertyBuilder : ISettingsPropertyBuilder
        {
            if (!Properties.ContainsKey(builder.Name))
                Properties[builder.Name] = builder;
            return this;
        }

        public IPropertyGroupDefinition GetPropertyGroupDefinition() => new PropertyGroupDefinitionWrapper(this);
    }
}