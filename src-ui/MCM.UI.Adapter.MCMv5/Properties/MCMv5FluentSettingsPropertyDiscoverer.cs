using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions;
using MCM.Abstractions.Base;
using MCM.Abstractions.Properties;
using MCM.Abstractions.Wrapper;
using MCM.Common;

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MCM.UI.Adapter.MCMv5.Properties;

public sealed class MCMv5FluentSettingsPropertyDiscoverer : IAttributeSettingsPropertyDiscoverer
{
    private delegate IEnumerable? GetSettingPropertyGroupsDelegate(object instance);

    private static readonly ConcurrentDictionary<Type, GetSettingPropertyGroupsDelegate?> _cache = new();

    public IEnumerable<string> DiscoveryTypes { get; } = new[] { "mcm_v5_fluent" };

    public IEnumerable<ISettingsPropertyDefinition> GetProperties(BaseSettings settings)
    {
        var obj = settings switch
        {
            IWrapper { Object: { } obj2 } => obj2,
            _ => settings
        };

        var del = _cache.GetOrAdd(obj.GetType(), static x => AccessTools2.GetPropertyGetterDelegate<GetSettingPropertyGroupsDelegate>(x, "SettingPropertyGroups"));
        var settingPropertyGroups = del?.Invoke(obj)?.Cast<object>()
            .Select(x => new SettingsPropertyGroupDefinitionWrapper(x)).Cast<SettingsPropertyGroupDefinition>().ToList() ?? [];

        return settingPropertyGroups.SelectMany(SettingsUtils.GetAllSettingPropertyDefinitions);
    }
}