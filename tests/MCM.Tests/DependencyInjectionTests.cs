using MCM.Abstractions.Settings.Formats;
using MCM.DependencyInjection;
using MCM.Implementation.Settings.Formats.Json;
using MCM.Implementation.Settings.Formats.Xml;

using NUnit.Framework;

using System.Collections.Generic;
using System.Linq;

namespace MCM.Tests
{
    public class DependencyInjectionTests : BaseTests
    {
        [Test]
        public void Resolve_Test()
        {
            var implementations = GenericServiceProvider.GetService<IEnumerable<ISettingsFormat>>()?.ToList() ?? new List<ISettingsFormat>();
            Assert.True(implementations.Any(i => i is MemorySettingsFormat), "MemorySettingsFormat missing");
            Assert.True(implementations.Any(i => i is JsonSettingsFormat), "JsonSettingsFormat missing");
            Assert.True(implementations.Any(i => i is XmlSettingsFormat), "XmlSettingsFormat missing");
        }
    }
}