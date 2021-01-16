using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Definitions;

using System.Collections.Generic;

namespace MCM.Abstractions.FluentBuilder
{
    // Setter/Getter
    public interface ISettingsPropertyBuilder
    {
        string Name { get; }
        IRef PropertyReference { get; }

        IEnumerable<IPropertyDefinitionBase> GetDefinitions();
    }

    public interface ISettingsPropertyBuilder<out TSettingsPropertyBuilder> : ISettingsPropertyBuilder
        where TSettingsPropertyBuilder : ISettingsPropertyBuilder
    {
        TSettingsPropertyBuilder SetOrder(int value);
        TSettingsPropertyBuilder SetRequireRestart(bool value);
        TSettingsPropertyBuilder SetHintText(string value);
    }
}