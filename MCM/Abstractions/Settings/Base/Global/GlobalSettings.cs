using System;
using System.Collections.Concurrent;

using MCM.Abstractions.Settings.Providers;

namespace MCM.Abstractions.Settings.Base.Global
{
    public abstract class GlobalSettings<T> : GlobalSettings where T : GlobalSettings, new()
    {
        public static T? Instance
        {
            get
            {
                if (!_cache.ContainsKey(typeof(T)))
                    _cache.TryAdd(typeof(T), new T().Id);
                return BaseSettingsProvider.Instance.GetSettingsObject(_cache[typeof(T)]) as T;

            }
        }
    }

    public abstract class GlobalSettings : BaseSettings
    {
        protected static readonly ConcurrentDictionary<Type, string> _cache = new ConcurrentDictionary<Type, string>();
    }
}