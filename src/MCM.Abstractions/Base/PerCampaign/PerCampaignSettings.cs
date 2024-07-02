using System;
using System.Collections.Concurrent;

namespace MCM.Abstractions.Base.PerCampaign
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    abstract class PerCampaignSettings<T> : PerCampaignSettings where T : PerCampaignSettings, new()
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

        /*
        /// <summary>
        /// A modder flriendly way to get settings from any place
        /// We now cache it. Cache is invalidated on game start/end.
        /// </summary>
        public static T? Instance => (T?) CacheInstance.GetOrAdd(Cache.GetOrAdd(typeof(T), static _ => new T().Id), static id =>
        {
            return BaseSettingsProvider.Instance?.GetSettings(id!);
        });

        /// <summary>
        /// A slower way to get settings from any place.
        /// </summary>
        public static T? InstanceNotCached => (T?) BaseSettingsProvider.Instance?.GetSettings(Cache.GetOrAdd(typeof(T), static _ =>
        {
            return new T().Id;
        })!);
        */
    }

#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    abstract class PerCampaignSettings : BaseSettings
    {
        protected static readonly ConcurrentDictionary<Type, string> Cache = new();
        protected static readonly ConcurrentDictionary<string, BaseSettings?> CacheInstance = new();

        internal static void InvalidateCache()
        {
            Cache.Clear();
            CacheInstance.Clear();
        }

        public sealed override string FormatType { get; } = "json2";
    }
}