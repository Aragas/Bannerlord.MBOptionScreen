using MCM.Abstractions.Settings.Formats;
using MCM.Abstractions.Settings.Formats.Memory;
using MCM.Implementation;
using MCM.Implementation.Settings.Formats.Json;
using MCM.Implementation.Settings.Formats.Xml;
using MCM.Utils;

using NUnit.Framework;

using System.Linq;

namespace MCM.Tests
{
    public class DITests
    {
        [SetUp]
        public void Setup()
        {
            // Force load Implementation assembly
            var type = typeof(MCMImplementationSubModule);
        }

        [Test]
        public void VersionResolve_Test()
        {
            var implementations = DI.GetBaseImplementations<ISettingsFormat>().ToList();
            Assert.True(implementations.Any(i => i is MemorySettingsFormat));
            Assert.True(implementations.Any(i => i is JsonSettingsFormat));
            Assert.True(implementations.Any(i => i is XmlSettingsFormat));
        }
    }
}