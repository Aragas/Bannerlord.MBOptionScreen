using HarmonyLib;

using System;
using System.Collections.Generic;
using System.Reflection;

using TaleWorlds.MountAndBlade;

namespace MCM.Abstractions.Loader
{
    public class IntegratedLoaderWrapper : IIntegratedLoader, IWrapper
    {
        /// <inheritdoc/>
        public object Object { get; }
        private PropertyInfo? MCMImplementationSubModulesProperty { get; }
        private MethodInfo? LoadMethod { get; }
        /// <inheritdoc/>
        public bool IsCorrect { get; }

        public IntegratedLoaderWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();

            MCMImplementationSubModulesProperty = AccessTools.Property(type, nameof(MCMImplementationSubModules));
            LoadMethod = AccessTools.Method(type, nameof(Load));

            IsCorrect = MCMImplementationSubModulesProperty != null && LoadMethod != null;
        }

        /// <inheritdoc/>
        public List<(MBSubModuleBase, Type)> MCMImplementationSubModules =>
            MCMImplementationSubModulesProperty?.GetValue(Object) as List<(MBSubModuleBase, Type)> ?? new List<(MBSubModuleBase, Type)>();
        /// <inheritdoc/>
        public void Load() => LoadMethod?.Invoke(Object, Array.Empty<object>());
    }
}