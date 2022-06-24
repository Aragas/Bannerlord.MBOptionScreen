using MCM.Abstractions.Providers;

using System;
using System.Collections.Concurrent;

namespace MCM.Abstractions.Base.PerSave
{
    public abstract class PerSaveSettings<T> : PerSaveSettings where T : PerSaveSettings, new()
    {
        /// <summary>
        /// A modder flriendly way to get settings from any place
        /// </summary>
        public static T? Instance
        {
            get
            {
                if (!Cache.ContainsKey(typeof(T)))
                    Cache.TryAdd(typeof(T), new T().Id);
                return BaseSettingsProvider.Instance?.GetSettings(Cache[typeof(T)]) as T;
            }
        }
    }

    public abstract class PerSaveSettings : BaseSettings
    {
        protected static readonly ConcurrentDictionary<Type, string> Cache = new();

        public sealed override string FormatType { get; } = "json2";
    }
}