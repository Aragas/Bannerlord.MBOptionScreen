using System;
using System.Linq;
using MCM.Abstractions.Settings.Formats;
using MCM.Utils;
using TaleWorlds.Library;
using Xunit;

namespace MCM.Tests
{
    public class VersionUtilsTests
    {
        [Fact]
        public void Test1()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .SelectMany(a => a.GetTypes())
                .Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(ISettingsFormat)));

            var impl =VersionUtils.GetLastImplementation(new ApplicationVersion(ApplicationVersionType.EarlyAccess, 1, 0, 0, 0), types);
            ;
        }
    }
}