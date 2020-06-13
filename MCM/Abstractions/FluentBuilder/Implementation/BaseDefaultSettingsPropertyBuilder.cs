using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Definitions;

using System.Collections.Generic;

namespace MCM.Abstractions.FluentBuilder.Implementation
{
    public abstract class BaseDefaultSettingsPropertyBuilder<TSettingsPropertyBuilder> :
        ISettingsPropertyBuilder<TSettingsPropertyBuilder>,
        IPropertyDefinitionBase,
        IPropertyDefinitionWithId
        where TSettingsPropertyBuilder : ISettingsPropertyBuilder
    {
        protected TSettingsPropertyBuilder SettingsPropertyBuilder { get; set; } = default!;

        public string Name { get; }
        public string Id { get; }
        public IRef PropertyReference { get; }

        public string DisplayName => Name;
        public int Order { get; private set; }
        public bool RequireRestart { get; private set; }
        public string HintText { get; private set; } = "";

        protected BaseDefaultSettingsPropertyBuilder(string id, string name, IRef @ref)
        {
            Id = id;
            Name = name;
            PropertyReference = @ref;
        }

        public TSettingsPropertyBuilder SetOrder(int value) { Order = value; return SettingsPropertyBuilder; }
        public TSettingsPropertyBuilder SetRequireRestart(bool value) { RequireRestart = value; return SettingsPropertyBuilder; }
        public TSettingsPropertyBuilder SetHintText(string value) { HintText = value; return SettingsPropertyBuilder; }

        public abstract IEnumerable<IPropertyDefinitionBase> GetDefinitions();
    }
}