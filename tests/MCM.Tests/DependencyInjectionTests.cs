using Bannerlord.ButterLib.Common.Extensions;
using MCM.Abstractions.Settings.Formats;
using MCM.Implementation.Settings.Formats.Json;
using MCM.Implementation.Settings.Formats.Xml;

using Microsoft.Extensions.DependencyInjection;

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
            var implementations = MCMSubModule.Instance!.GetServiceProvider()!.GetRequiredService<IEnumerable<ISettingsFormat>>().ToList();
            Assert.True(implementations.Any(i => i is MemorySettingsFormat), "MemorySettingsFormat missing");
            Assert.True(implementations.Any(i => i is JsonSettingsFormat), "JsonSettingsFormat missing");
            Assert.True(implementations.Any(i => i is XmlSettingsFormat), "XmlSettingsFormat missing");
        }
    }
}