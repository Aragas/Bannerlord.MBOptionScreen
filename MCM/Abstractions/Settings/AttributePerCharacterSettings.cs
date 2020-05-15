using MCM.Abstractions.Settings.Models;
using MCM.Abstractions.Settings.Properties;
using MCM.Utils;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings
{
    public abstract class AttributePerCharacterSettings<T> : PerCharacterSettings<T> where T : PerCharacterSettings, new()
    {
        protected ISettingsPropertyDiscoverer? Discoverer { get; }
            = DI.GetImplementation<IAttributeSettingsPropertyDiscoverer, AttributeSettingsPropertyDiscovererWrapper>();

        protected override IEnumerable<SettingsPropertyGroupDefinition> GetUnsortedSettingPropertyGroups()
        {
            //var Discoverer = DI.GetImplementation<IAttributeSettingsPropertyDiscoverer, AttributeSettingsPropertyDiscovererWrapper>();
            var groups = new List<SettingsPropertyGroupDefinition>();
            if (Discoverer == null)
                return groups;

            foreach (var settingProp in Discoverer.GetProperties(this))
            {
                var group = SettingsUtils.GetGroupFor(SubGroupDelimiter, settingProp, groups);
                group.Add(settingProp);
            }
            return groups;
        }
    }
}