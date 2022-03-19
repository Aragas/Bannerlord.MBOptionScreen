extern alias v4;

using Bannerlord.ButterLib;
using Bannerlord.ButterLib.SubModuleWrappers;

using HarmonyLib;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using TaleWorlds.Engine;

using v4::Bannerlord.ModuleManager;
using v4::Bannerlord.BUTR.Shared.Helpers;

using v4::MCM;
using v4::MCM.Implementation;

using AccessTools2 = v4::HarmonyLib.BUTR.Extensions.AccessTools2;
using SymbolExtensions2 = v4::HarmonyLib.BUTR.Extensions.SymbolExtensions2;

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
                prefix: new HarmonyMethod(DelegateHelper.GetMethodInfo(MockedGetConfigsPath)));
            _harmony.Patch(SymbolExtensions2.GetMethodInfo(() => FSIOHelper.GetConfigPath()),
                prefix: new HarmonyMethod(DelegateHelper.GetMethodInfo(MockedGetConfigsPath)));
            _harmony.Patch(SymbolExtensions2.GetMethodInfo(() => ModuleInfoHelper.GetLoadedModules()),
                prefix: new HarmonyMethod(DelegateHelper.GetMethodInfo(MockedGetLoadedModules)));
            _harmony.Patch(SymbolExtensions2.GetMethodInfo(() => Utilities.GetModulesNames()),
                prefix: new HarmonyMethod(DelegateHelper.GetMethodInfo(MockedGetModulesNames)));

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
            _harmony.Unpatch(SymbolExtensions2.GetMethodInfo(() => FSIOHelper.GetConfigPath()),
                DelegateHelper.GetMethodInfo(MockedGetConfigsPath));
            _harmony.Unpatch(SymbolExtensions2.GetMethodInfo(() => ModuleInfoHelper.GetLoadedModules()),
                DelegateHelper.GetMethodInfo(MockedGetLoadedModules));
            _harmony.Unpatch(SymbolExtensions2.GetMethodInfo(() => Utilities.GetModulesNames()),
                DelegateHelper.GetMethodInfo(MockedGetModulesNames));
        }
    }
}