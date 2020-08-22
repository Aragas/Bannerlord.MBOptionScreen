using Bannerlord.ButterLib.Common.Extensions;

using MCM.Abstractions.Settings.Providers;

using System;
using System.Collections.Concurrent;

using TaleWorlds.CampaignSystem;

namespace MCM.Abstractions.Settings.Base.PerCampaign
{
    public abstract class PerCampaignSettings<T> : PerCampaignSettings where T : PerCampaignSettings, new()
    {
        public static T? Instance
        {
            get
            {
                if (!Cache.ContainsKey(typeof(T)))
                    Cache.TryAdd(typeof(T), new T().Id);
                return BaseSettingsProvider.Instance.GetSettingsObject(Cache[typeof(T)]) as T;
            }
        }
    }

    public abstract class PerCampaignSettings : BaseSettings
    {
        protected static readonly ConcurrentDictionary<Type, string> Cache = new ConcurrentDictionary<Type, string>();

        public string CampaignId { get; } = Campaign.Current.GetCampaignId() ?? "ERROR";
    }
}