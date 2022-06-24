extern alias v5;

using NUnit.Framework;

using System.Collections.Generic;
using System.Linq;

using v5::BUTR.DependencyInjection;
using v5::MCM.Abstractions.Settings.Formats;
using v5::MCM.Implementation.Settings.Formats;

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