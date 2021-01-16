using System;
using System.Collections.Concurrent;

namespace MCM.Abstractions.Settings.Base.Global
{
    public abstract class GlobalSettings<T> : GlobalSettings where T : GlobalSettings, new()
    {
        public static T? Instance => throw new NotImplementedException();
    }

    public abstract class GlobalSettings : BaseSettings
    {
        protected static readonly ConcurrentDictionary<Type, string> Cache = new();
    }
}