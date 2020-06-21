using MCM.Abstractions.Settings.Formats;
using MCM.Utils;

using NUnit.Framework;

using System;
using System.Linq;

using TaleWorlds.Library;

namespace MCM.Tests
{
    public class VersionUtilsTests
    {
        [Test]
        public void Test1()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .SelectMany(a => a.GetTypes())
                .Where(t => ReflectionUtils.ImplementsOrImplementsEquivalent(t, typeof(ISettingsFormat)));

            var impl = VersionUtils.GetLastImplementation(new ApplicationVersion(ApplicationVersionType.EarlyAccess, 1, 0, 0, 0), types);
            ;
        }
    }
}