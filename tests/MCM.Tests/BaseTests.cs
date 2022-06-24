extern alias v5;

using Bannerlord.ButterLib;
using Bannerlord.ButterLib.SubModuleWrappers;

using HarmonyLib;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using TaleWorlds.Engine;

using v5::Bannerlord.BUTR.Shared.Helpers;
using v5::Bannerlord.ModuleManager;
using v5::MCM;
using v5::MCM.Implementation;

using AccessTools2 = v5::HarmonyLib.BUTR.Extensions.AccessTools2;
using SymbolExtensions2 = v5::HarmonyLib.BUTR.Extensions.SymbolExtensions2;

namespace MCM.Tests
{
    public class BaseTests
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool MockedGetConfigsPath(ref string __result)
        {
            __result = AppDomain.CurrentDomain.BaseDirectory;
            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool MockedGetLoadedModules(ref IEnumerable<ModuleInfoExtended> __result)
        {
            __result = new List<ModuleInfoExtended>();
            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool MockedGetModulesNames(ref string[] __result)
        {
            __result = Array.Empty<string>();
            return false;
        }

        private readonly Harmony _harmony = new(nameof(BaseTests));

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _harmony.Patch(AccessTools2.Method(typeof(ButterLibSubModule).Assembly.GetType("Bannerlord.BUTR.Shared.Helpers.FSIOHelper"), "GetConfigPath"),
                prefix: new HarmonyMethod(SymbolExtensions2.GetMethodInfo((string x) => MockedGetConfigsPath(ref x))));
            _harmony.Patch(SymbolExtensions2.GetMethodInfo(() => FSIOHelper.GetConfigPath()),
                prefix: new HarmonyMethod(SymbolExtensions2.GetMethodInfo((string x) => MockedGetConfigsPath(ref x))));
            _harmony.Patch(SymbolExtensions2.GetMethodInfo(() => ModuleInfoHelper.GetLoadedModules()),
                prefix: new HarmonyMethod(SymbolExtensions2.GetMethodInfo((IEnumerable<ModuleInfoExtended> x) => MockedGetLoadedModules(ref x))));
            _harmony.Patch(SymbolExtensions2.GetMethodInfo(() => Utilities.GetModulesNames()),
                prefix: new HarmonyMethod(SymbolExtensions2.GetMethodInfo((string[] x) => MockedGetModulesNames(ref x))));

            var butterLib = new MBSubModuleBaseWrapper(new ButterLibSubModule());
            butterLib.SubModuleLoad();
            var mcm = new MBSubModuleBaseWrapper(new MCMSubModule());
            mcm.SubModuleLoad();
            new MBSubModuleBaseWrapper(new MCMImplementationSubModule()).SubModuleLoad();
            butterLib.BeforeInitialModuleScreenSetAsRoot();
            mcm.BeforeInitialModuleScreenSetAsRoot();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _harmony.UnpatchAll(_harmony.Id);
        }
    }
}