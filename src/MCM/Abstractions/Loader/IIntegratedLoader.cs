using System;
using System.Collections.Generic;

using TaleWorlds.MountAndBlade;

namespace MCM.Abstractions.Loader
{
    public interface IIntegratedLoader : IDependency
    {
        List<(MBSubModuleBase, Type)> MCMImplementationSubModules { get; }
        void Load();
    }
}