using MCM.Abstractions.Settings.Providers;

using System;
using System.Collections.Concurrent;

using TaleWorlds.Core;

namespace MCM.Abstractions.Settings.Base.PerCharacter
{
    public abstract class PerCharacterSettings<T> : PerCharacterSettings where T : PerCharacterSettings, new()
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

    public abstract class PerCharacterSettings : BaseSettings
    {
        protected static readonly ConcurrentDictionary<Type, string> _cache = new ConcurrentDictionary<Type, string>();

        public string CharacterId { get; } = $"{Game.Current?.PlayerTroop?.Id.ToString() ?? "ERROR"}_{Game.Current?.PlayerTroop?.Name.ToString() ?? "ERROR"}";
    }
}