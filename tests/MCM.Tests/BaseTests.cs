extern alias v4;

using Bannerlord.ButterLib;
using Bannerlord.ButterLib.SubModuleWrappers2;

using HarmonyLib;

using NUnit.Framework;

using System;
using System.Linq;
using System.Runtime.CompilerServices;

using TaleWorlds.Engine;

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
        private static bool MockedGetModulesNames(ref string[] __result)
        {
            __result = Array.Empty<string>();
            return false;
        }

        private readonly Harmony _harmony = new(nameof(BaseTests));

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            foreach (var fsioHelper in AccessTools2.AllTypes().Where(x => x.FullName == "Bannerlord.BUTR.Shared.Helpers.FSIOHelper"))
            {
                _harmony.Patch(AccessTools2.Method(fsioHelper, "GetConfigPath"),
                    prefix: new HarmonyMethod(SymbolExtensions2.GetMethodInfo((string x) => MockedGetConfigsPath(ref x))));
            }
            _harmony.Patch(SymbolExtensions2.GetMethodInfo(() => Utilities.GetModulesNames()),
                prefix: new HarmonyMethod(SymbolExtensions2.GetMethodInfo((string[] x) => MockedGetModulesNames(ref x))));

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