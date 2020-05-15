using HarmonyLib;

using System.Reflection;

using TaleWorlds.Core;

namespace MCM.Abstractions.Settings.SettingsContainer
{
    public abstract class BasePerCharacterSettingsContainerWrapper : BaseSettingsContainerWrapper, IPerCharacterSettingsContainer
    {
        private MethodInfo? OnGameStartedMethod { get; }
        private MethodInfo? OnGameEndedMethod { get; }
        public override bool IsCorrect { get; }

        public BasePerCharacterSettingsContainerWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();
            OnGameStartedMethod = AccessTools.Method(type, nameof(OnGameStarted));
            OnGameEndedMethod = AccessTools.Method(type, nameof(OnGameEnded));

            IsCorrect = base.IsCorrect && OnGameStartedMethod != null && OnGameEndedMethod != null;
        }

        public void OnGameStarted(Game game) => OnGameStartedMethod?.Invoke(Object, new object[] { game });
        public void OnGameEnded(Game game) => OnGameEndedMethod?.Invoke(Object, new object[] { game });
    }
}