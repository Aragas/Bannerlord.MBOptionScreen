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
        [Obsolete("Will be removed", true)]
        public bool IsMainToggle { get; private set; }
        public int GroupOrder { get; private set; }
        private bool HasGroupToggle { get; set; }

        public DefaultSettingsPropertyGroupBuilder(string name)
        {
            GroupName = name;
        }

        public ISettingsPropertyGroupBuilder SetIsMainToggle(bool value) { return this; }
        public ISettingsPropertyGroupBuilder SetGroupOrder(int value) { GroupOrder = value; return this; }

        public ISettingsPropertyGroupBuilder AddToggle(string id, string name, IRef @ref, Action<ISettingsPropertyGroupToggleBuilder>? builder)
        {
            if (HasGroupToggle)
                throw new InvalidOperationException("There already exists a group toggle property!");

            HasGroupToggle = true;

            if (!Properties.ContainsKey(name))
                Properties[name] = new DefaultSettingsPropertyGroupToggleBuilder(id, name, @ref);
            builder?.Invoke((ISettingsPropertyGroupToggleBuilder) Properties[name]);
            return this;

        }

        public ISettingsPropertyGroupBuilder AddBool(string id, string name, IRef @ref, Action<ISettingsPropertyBoolBuilder>? builder)
        {
            if (!Properties.ContainsKey(name))
                Properties[name] = new DefaultSettingsPropertyBoolBuilder(id, name, @ref);
            builder?.Invoke((ISettingsPropertyBoolBuilder) Properties[name]);
            return this;
        }
        public ISettingsPropertyGroupBuilder AddDropdown(string id, string name, int selectedIndex, IRef @ref, Action<ISettingsPropertyDropdownBuilder>? builder)
        {
            if (!Properties.ContainsKey(name))
                Properties[name] = new DefaultSettingsPropertyDropdownBuilder(id, name, selectedIndex, @ref);
            builder?.Invoke((ISettingsPropertyDropdownBuilder) Properties[name]);
            return this;
        }
        public ISettingsPropertyGroupBuilder AddInteger(string id, string name, int minValue, int maxValue, IRef @ref, Action<ISettingsPropertyIntegerBuilder>? builder)
        {
            if (!Properties.ContainsKey(name))
                Properties[name] = new DefaultSettingsPropertyIntegerBuilder(id, name, minValue, maxValue, @ref);
            builder?.Invoke((ISettingsPropertyIntegerBuilder) Properties[name]);
            return this;
        }
        public ISettingsPropertyGroupBuilder AddFloatingInteger(string id, string name, float minValue, float maxValue, IRef @ref, Action<ISettingsPropertyFloatingIntegerBuilder>? builder)
        {
            if (!Properties.ContainsKey(name))
                Properties[name] = new DefaultSettingsPropertyFloatingIntegerBuilder(id, name, minValue, maxValue, @ref);
            builder?.Invoke((ISettingsPropertyFloatingIntegerBuilder) Properties[name]);
            return this;
        }
        public ISettingsPropertyGroupBuilder AddText(string id, string name, IRef @ref, Action<ISettingsPropertyTextBuilder>? builder)
        {
            if (!Properties.ContainsKey(name))
                Properties[name] = new DefaultSettingsPropertyTextBuilder(id, name, @ref);
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