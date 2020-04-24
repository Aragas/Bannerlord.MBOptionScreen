using HarmonyLib;

using System.Reflection;

using TaleWorlds.Library;

namespace MBOptionScreen
{
    internal sealed class MBOptionScreenInitializerWrapper : IMBOptionScreenInitializer, IWrapper
    {
        private readonly object _object;
        private MethodInfo StartInitializationMethod { get; }
        private MethodInfo EndInitializationMethod { get; }
        public bool IsCorrect { get; }

        public MBOptionScreenInitializerWrapper(object @object)
        {
            _object = @object;
            var type = @object.GetType();

            StartInitializationMethod = AccessTools.Method(type, "StartInitialization");
            EndInitializationMethod = AccessTools.Method(type, "EndInitialization");

            IsCorrect = StartInitializationMethod != null && EndInitializationMethod != null;
        }

        public void StartInitialization(ApplicationVersion gameVerion, bool first) =>
            StartInitializationMethod.Invoke(_object, new object[] { gameVerion, first });

        public void EndInitialization(bool first) =>
            EndInitializationMethod.Invoke(_object, new object[] { first });
    }
}