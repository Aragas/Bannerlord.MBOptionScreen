using System.Collections.Generic;
using MCM.Abstractions.Settings.Formats;
using MCM.Abstractions.Settings.Formats.Memory;
using MCM.Implementation;
using MCM.Implementation.Settings.Formats.Json;
using MCM.Implementation.Settings.Formats.Xml;
using MCM.Utils;

using NUnit.Framework;

using System.Linq;
using System.Runtime.CompilerServices;
using Bannerlord.ButterLib;
using Bannerlord.ButterLib.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace MCM.Tests
{
    public class DITests
    {
        private bool @bool;

        [SetUp]
        public void Setup()
        {
            // Force load Implementation assembly
            // Avoiding compiler optimization))0)
            @bool = typeof(MCMImplementationSubModule) != null;
        }

        [Test]
        public void VersionResolve_Test()
        {
            var implementations = ButterLibSubModule.Instance.GetServiceProvider().GetRequiredService<IEnumerable<ISettingsFormat>>().ToList();
            //var implementations = DI.GetBaseImplementations<ISettingsFormat>().ToList();
            Assert.True(implementations.Any(i => i is MemorySettingsFormat), "MemorySettingsFormat missing");
            Assert.True(implementations.Any(i => i is JsonSettingsFormat), "JsonSettingsFormat missing");
            Assert.True(implementations.Any(i => i is XmlSettingsFormat), "XmlSettingsFormat missing");
            Assert.True(@bool);
        }
    }
}
