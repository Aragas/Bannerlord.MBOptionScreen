using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions;
using MCM.Abstractions.Base;
using MCM.Abstractions.Properties;
using MCM.Abstractions.Wrapper;

using System.Collections.Generic;
using System.Linq;

namespace MCM.Implementation
{
    internal sealed class FluentSettingsPropertyDiscoverer : IFluentSettingsPropertyDiscoverer
    {
        private delegate List<SettingsPropertyGroupDefinition> GetSettingPropertyGroupsDelegate(object instance);

        private static readonly GetSettingPropertyGroupsDelegate? _getSettingPropertyGroups =
            AccessTools2.GetPropertyGetterDelegate<GetSettingPropertyGroupsDelegate>("MCM.Abstractions.Base.IFluentSettings:SettingPropertyGroups");

        public IEnumerable<string> DiscoveryTypes { get; } = new[] { "fluent" };

        public IEnumerable<ISettingsPropertyDefinition> GetProperties(BaseSettings settings)
        {
            var settingPropertyGroups = _getSettingPropertyGroups?.Invoke(settings)
                .Select(x => new SettingsPropertyGroupDefinitionWrapper(x)).Cast<SettingsPropertyGroupDefinition>().ToList() ?? [];

            return settingPropertyGroups.SelectMany(SettingsUtils.GetAllSettingPropertyDefinitions);
        }
    }
}