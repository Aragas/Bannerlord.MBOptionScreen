using MCM.Abstractions;
using MCM.Abstractions.Base;

using System;
using System.Collections.Concurrent;

using TaleWorlds.CampaignSystem;

namespace MCM
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    class SettingsProviderCampaignBehavior : CampaignBehaviorBase
    {
        private readonly BaseSettingsProvider? _baseSettingsProvider;
        private readonly ConcurrentDictionary<Type, string> _cache = new();

        public SettingsProviderCampaignBehavior(BaseSettingsProvider? baseSettingsProvider)
        {
            _baseSettingsProvider = baseSettingsProvider;
        }

        public TSettings? Get<TSettings>() where TSettings : BaseSettings, new()
        {
            if (!_cache.ContainsKey(typeof(TSettings)))
                _cache.TryAdd(typeof(TSettings), new TSettings().Id);
            return _baseSettingsProvider?.GetSettings(_cache[typeof(TSettings)]) as TSettings;
        }

        public override void RegisterEvents() { }
        public override void SyncData(IDataStore dataStore) { }
    }
}