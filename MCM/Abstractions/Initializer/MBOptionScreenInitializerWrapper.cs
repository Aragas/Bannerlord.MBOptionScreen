using HarmonyLib;

using System.Reflection;

using TaleWorlds.Library;

namespace MCM.Abstractions.Initializer
{
    public sealed class MBOptionScreenInitializerWrapper : IMBOptionScreenInitializer, IWrapper
    {
        public object Object { get; }
        private MethodInfo? StartInitializationMethod { get; }
        private MethodInfo? EndInitializationMethod { get; }
        public bool IsCorrect { get; }

        public MBOptionScreenInitializerWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();

            StartInitializationMethod = AccessTools.Method(type, nameof(StartInitialization));
            EndInitializationMethod = AccessTools.Method(type, nameof(EndInitialization));

            IsCorrect = StartInitializationMethod != null && EndInitializationMethod != null;
        }

        public void StartInitialization(ApplicationVersion gameVersion, bool first) =>
            StartInitializationMethod?.Invoke(Object, new object[] { gameVersion, first });
        public void EndInitialization(bool first) =>
            EndInitializationMethod?.Invoke(Object, new object[] { first });
    }
}