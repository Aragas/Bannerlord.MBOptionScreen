using HarmonyLib;

using System.Reflection;

using TaleWorlds.Core;

namespace MCM.Abstractions.Settings.SettingsContainer
{
    public abstract class FluentPerCharacterSettingsContainerWrapper : BaseSettingsContainerWrapper, IFluentPerCharacterSettingsContainer
    {
        private MethodInfo? OnGameStartedMethod { get; }
        private MethodInfo? OnGameEndedMethod { get; }
        private MethodInfo? RegisterMethod { get; }
        private MethodInfo? UnregisterMethod { get; }
        public override bool IsCorrect { get; }

        protected FluentPerCharacterSettingsContainerWrapper(object @object) : base(@object)
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

        public void OnGameStarted(Game game) => OnGameStartedMethod?.Invoke(Object, new object[] { game });
        public void OnGameEnded(Game game) => OnGameEndedMethod?.Invoke(Object, new object[] { game });
        public void Register(FluentPerCharacterSettings settings) => RegisterMethod?.Invoke(Object, new object[] { settings });
        public void Unregister(FluentPerCharacterSettings settings) => UnregisterMethod?.Invoke(Object, new object[] { settings });
    }
}