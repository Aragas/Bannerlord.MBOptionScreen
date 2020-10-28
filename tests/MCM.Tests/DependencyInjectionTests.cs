using Bannerlord.ButterLib;
using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.ButterLib.SubModuleWrappers;

using HarmonyLib;

using MCM.Abstractions.Settings.Formats;
using MCM.Implementation;
using MCM.Implementation.Settings.Formats.Json;
using MCM.Implementation.Settings.Formats.Xml;

using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace MCM.Tests
{
    public class DependencyInjectionTests
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool MockedGetConfigsPath(ref string __result)
        {
            __result = AppDomain.CurrentDomain.BaseDirectory;
            return false;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool MockedGetLoadedModules(ref List<ModuleInfo> __result)
        {
            __result = new List<ModuleInfo>();
            return false;
        }

        [SetUp]
        public void Setup()
        {
            var harmony = new Harmony($"{nameof(DependencyInjectionTests)}.{nameof(Setup)}");
            harmony.Patch(SymbolExtensions.GetMethodInfo(() => Utilities.GetConfigsPath()),
                prefix: new HarmonyMethod(DelegateHelper.GetMethodInfo(MockedGetConfigsPath)));
            harmony.Patch(SymbolExtensions.GetMethodInfo(() => ModuleInfoHelper.GetLoadedModules()),
                prefix: new HarmonyMethod(DelegateHelper.GetMethodInfo(MockedGetLoadedModules)));

            var butterLib = new MBSubModuleBaseWrapper(new ButterLibSubModule());
            butterLib.SubModuleLoad();
            new MBSubModuleBaseWrapper(new MCMSubModule()).SubModuleLoad();
            new MBSubModuleBaseWrapper(new MCMImplementationSubModule()).SubModuleLoad();
            butterLib.BeforeInitialModuleScreenSetAsRoot();
        }

        [Test]
        public void Resolve_Test()
        {
            var implementations = MCMSubModule.Instance!.GetServiceProvider()!.GetRequiredService<IEnumerable<ISettingsFormat>>().ToList();
            Assert.True(implementations.Any(i => i is MemorySettingsFormat), "MemorySettingsFormat missing");
            Assert.True(implementations.Any(i => i is JsonSettingsFormat), "JsonSettingsFormat missing");
            Assert.True(implementations.Any(i => i is XmlSettingsFormat), "XmlSettingsFormat missing");
        }
    }
}