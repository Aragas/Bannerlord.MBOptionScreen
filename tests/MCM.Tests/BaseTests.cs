extern alias UI;
extern alias v5;

using Bannerlord.ButterLib;
using Bannerlord.ButterLib.SubModuleWrappers2;

using HarmonyLib;

using NUnit.Framework;

using System;
using System.Runtime.CompilerServices;

using TaleWorlds.Engine;
using TaleWorlds.Library;

using v5::MCM;
using v5::MCM.Internal;

using SymbolExtensions2 = v5::HarmonyLib.BUTR.Extensions.SymbolExtensions2;

namespace MCM.Tests
{
    public class BaseTests
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool MockedGetModulesNames(ref string[] __result)
        {
            __result = Array.Empty<string>();
            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool MockedPlatformFileHelper(ref IPlatformFileHelper __result)
        {
            __result = new PlatformFileHelperPC("");
            return false;
        }

        private readonly Harmony _harmony = new(nameof(BaseTests));

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            //_harmony.Patch(AccessTools2.Method(typeof(ButterLibSubModule).Assembly.GetType("Bannerlord.BUTR.Shared.Helpers.FSIOHelper"), "GetConfigPath"),
            //    prefix: new HarmonyMethod(SymbolExtensions2.GetMethodInfo((string x) => MockedGetConfigsPath(ref x))));
            //_harmony.Patch(SymbolExtensions2.GetMethodInfo(() => FSIOHelper.GetConfigPath()),
            //    prefix: new HarmonyMethod(SymbolExtensions2.GetMethodInfo((string x) => MockedGetConfigsPath(ref x))));
            //_harmony.Patch(SymbolExtensions2.GetMethodInfo(() => ModuleInfoHelper.GetLoadedModules()),
            //    prefix: new HarmonyMethod(SymbolExtensions2.GetMethodInfo((IEnumerable<ModuleInfoExtended> x) => MockedGetLoadedModules(ref x))));
            _harmony.Patch(SymbolExtensions2.GetMethodInfo(() => Utilities.GetModulesNames()),
                prefix: new HarmonyMethod(SymbolExtensions2.GetMethodInfo((string[] x) => MockedGetModulesNames(ref x))));
            _harmony.Patch(SymbolExtensions2.GetPropertyGetter(() => TaleWorlds.Library.Common.PlatformFileHelper),
                prefix: new HarmonyMethod(SymbolExtensions2.GetMethodInfo((IPlatformFileHelper x) => MockedPlatformFileHelper(ref x))));

            var butterLib = new MBSubModuleBaseWrapper(new ButterLibSubModule());
            butterLib.OnSubModuleLoad();
            var mcm = new MBSubModuleBaseWrapper(new MCMSubModule());
            mcm.OnSubModuleLoad();
            new MBSubModuleBaseWrapper(new MCMImplementationSubModule()).OnSubModuleLoad();
            butterLib.OnBeforeInitialModuleScreenSetAsRoot();
            mcm.OnBeforeInitialModuleScreenSetAsRoot();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _harmony.UnpatchAll(_harmony.Id);
        }
    }
}