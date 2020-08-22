using HarmonyLib;

using MCM.Abstractions.Settings.Base.PerCampaign;

using System.Reflection;

using TaleWorlds.Core;

namespace MCM.Abstractions.Settings.Containers.PerCampaign
{
    public abstract class FluentPerCampaignSettingsContainerWrapper : BaseSettingsContainerWrapper, IFluentPerCampaignSettingsContainer
    {
        private MethodInfo? OnGameStartedMethod { get; }
        private MethodInfo? OnGameEndedMethod { get; }
        private MethodInfo? RegisterMethod { get; }
        private MethodInfo? UnregisterMethod { get; }
        /// <inheritdoc/>
        public override bool IsCorrect { get; }

        protected FluentPerCampaignSettingsContainerWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            OnGameStartedMethod = AccessTools.Method(type, nameof(OnGameStarted));
            OnGameEndedMethod = AccessTools.Method(type, nameof(OnGameEnded));
            RegisterMethod = AccessTools.Method(type, nameof(Register));
            UnregisterMethod = AccessTools.Method(type, nameof(Unregister));

            IsCorrect = base.IsCorrect &&
                        OnGameStartedMethod != null && OnGameEndedMethod != null &&
                        RegisterMethod != null && UnregisterMethod != null;
        }

        /// <inheritdoc/>
        public void OnGameStarted(Game game) => OnGameStartedMethod?.Invoke(Object, new object[] { game });
        /// <inheritdoc/>
        public void OnGameEnded(Game game) => OnGameEndedMethod?.Invoke(Object, new object[] { game });
        public void Register(FluentPerCampaignSettings settings) => RegisterMethod?.Invoke(Object, new object[] { settings });
        public void Unregister(FluentPerCampaignSettings settings) => UnregisterMethod?.Invoke(Object, new object[] { settings });
    }
}