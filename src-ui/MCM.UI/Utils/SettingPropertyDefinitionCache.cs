using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions;
using MCM.Abstractions.Base;

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MCM.UI.Utils;

internal static class SettingPropertyDefinitionCache
{
    private delegate void ClearDelegate(ConditionalWeakTable<BaseSettings, List<SettingsPropertyGroupDefinition>> instance);
    private static readonly ClearDelegate? ClearMethod =
        AccessTools2.GetDelegate<ClearDelegate>(typeof(ConditionalWeakTable<BaseSettings, List<SettingsPropertyGroupDefinition>>), "Clear");

    private static readonly ConditionalWeakTable<BaseSettings, List<SettingsPropertyGroupDefinition>> _cache = new();

    public static void Clear()
    {
        ClearMethod?.Invoke(_cache);
    }

    public static IEnumerable<SettingsPropertyGroupDefinition> GetSettingPropertyGroups(BaseSettings settings)
    {
        if (!_cache.TryGetValue(settings, out var list))
        {
            list = settings.GetSettingPropertyGroups();
            _cache.Add(settings, list);
        }
        return list;
    }

    public static IEnumerable<ISettingsPropertyDefinition> GetAllSettingPropertyDefinitions(BaseSettings settings) =>
        GetSettingPropertyGroups(settings).SelectMany(SettingsUtils.GetAllSettingPropertyDefinitions);
    public static IEnumerable<SettingsPropertyGroupDefinition> GetAllSettingPropertyGroupDefinitions(BaseSettings settings) =>
        GetSettingPropertyGroups(settings).SelectMany(SettingsUtils.GetAllSettingPropertyGroupDefinitions);

    public static bool Equals(BaseSettings settings1, BaseSettings settings2)
    {
        var setDict1 = GetAllSettingPropertyDefinitions(settings1).ToDictionary(x => (x.DisplayName, x.GroupName), x => x);
        var setDict2 = GetAllSettingPropertyDefinitions(settings2).ToDictionary(x => (x.DisplayName, x.GroupName), x => x);

        if (setDict1.Count != setDict2.Count)
            return false;

        foreach (var kv in setDict1)
        {
            if (!setDict2.TryGetValue(kv.Key, out var spd2) || !SettingsUtils.Equals(kv.Value, spd2))
                return false;
        }

        return true;
    }
}