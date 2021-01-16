using System;
using System.Collections.Generic;

using TaleWorlds.MountAndBlade;

namespace MCM.Abstractions.Loader
{
    public class IntegratedLoaderWrapper : IIntegratedLoader, IWrapper
    {
        public object Object { get; }
        public bool IsCorrect { get; }

        public IntegratedLoaderWrapper(object @object) { }

        public List<(MBSubModuleBase, Type)> MCMImplementationSubModules => new();
        public void Load() { }
    }
}