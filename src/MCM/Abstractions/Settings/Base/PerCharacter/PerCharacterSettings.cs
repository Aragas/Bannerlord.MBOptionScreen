using System;
using System.Collections.Concurrent;

namespace MCM.Abstractions.Settings.Base.PerCharacter
{
    public abstract class PerCharacterSettings<T> : PerCharacterSettings where T : PerCharacterSettings, new()
    {
        public static T? Instance => throw new NotImplementedException();
    }

    public abstract class PerCharacterSettings : BaseSettings
    {
        protected static readonly ConcurrentDictionary<Type, string> Cache = new();

        public string CharacterId { get; } = "ERROR";
    }
}