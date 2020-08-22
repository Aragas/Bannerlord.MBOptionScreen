using HarmonyLib;

using System.Reflection;

using TaleWorlds.Core;

namespace MCM.Abstractions.Settings.Containers.PerCampaign
{
    public abstract class BasePerCampaignSettingsContainerWrapper : BaseSettingsContainerWrapper, IPerCampaignSettingsContainer
    {
        private MethodInfo? OnGameStartedMethod { get; }
        private MethodInfo? OnGameEndedMethod { get; }
        /// <inheritdoc/>
        public override bool IsCorrect { get; }

        protected BasePerCampaignSettingsContainerWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();
            OnGameStartedMethod = AccessTools.Method(type, nameof(OnGameStarted));
            OnGameEndedMethod = AccessTools.Method(type, nameof(OnGameEnded));

            IsCorrect = base.IsCorrect && OnGameStartedMethod != null && OnGameEndedMethod != null;
        }

        /// <inheritdoc/>
        public void OnGameStarted(Game game) => OnGameStartedMethod?.Invoke(Object, new object[] { game });
        /// <inheritdoc/>
        public void OnGameEnded(Game game) => OnGameEndedMethod?.Invoke(Object, new object[] { game });
    }
}