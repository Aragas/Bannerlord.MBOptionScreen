using MCM.Abstractions.FluentBuilder.Implementation;
using MCM.Abstractions.Ref;
using MCM.Implementation.Settings.Formats.Json;

using NUnit.Framework;

using System;
using System.IO;

namespace MCM.Tests.SettingsFormat
{
    public class JsonSettingsFormatTests : BaseSettingsFormatTests
    {
        private static string Expected { get; } = @"{
  ""prop_4"": ""Test"",
  ""prop_3"": 5.3453,
  ""prop_2"": 5,
  ""prop_1"": true
}";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Format = new JsonSettingsFormat();

            Settings = new DefaultSettingsBuilder("Testing_Global_v1", "Testing Fluent Settings")
                .SetFormat("json")
                .SetFolderName(string.Empty)
                .SetSubFolder(string.Empty)
                .CreateGroup("Testing 1", groupBuilder => groupBuilder
                    .AddBool("prop_1", "Check Box", new ProxyRef<bool>(() => _boolValue, o => _boolValue = o), boolBuilder => boolBuilder
                        .SetHintText("Test")
                        .SetRequireRestart(false)))
                .CreateGroup("Testing 2", groupBuilder => groupBuilder
                    .AddInteger("prop_2","Integer", 0, 10, new ProxyRef<int>(() => _intValue, o => _intValue = o), integerBuilder => integerBuilder
                        .SetHintText("Testing"))
                    .AddFloatingInteger("prop_3","Floating Integer", 0, 10, new ProxyRef<float>(() => _floatValue, o => _floatValue = o), floatingBuilder => floatingBuilder
                        .SetRequireRestart(true)
                        .SetHintText("Test")))
                .CreateGroup("Testing 3", groupBuilder => groupBuilder
                    .AddText("prop_4","Test", new ProxyRef<string>(() => _stringValue, o => _stringValue = o), null))
                .BuildAsGlobal();

            // TODO: Should Load/Save accept the full path or just the base dir path?
            Path = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                Settings.FolderName,
                Settings.SubFolder ?? string.Empty,
                $"{Settings.Id}.{Settings.Format}");
        }

        [Test]
        public void Serialize_Test()
        {
            Assert.AreEqual(false, _boolValue);
            Assert.AreEqual(0, _intValue);
            Assert.AreEqual(0F, _floatValue);
            Assert.AreEqual(string.Empty, _stringValue);

            Format.Save(Settings, Path);
            Format.Load(Settings, Path);

            Assert.AreEqual(false, _boolValue);
            Assert.AreEqual(0, _intValue);
            Assert.AreEqual(0F, _floatValue);
            Assert.AreEqual(string.Empty, _stringValue);


            _boolValue = true;
            _intValue = 5;
            _floatValue = 5.3453F;
            _stringValue = "Test";

            Format.Save(Settings, Path);
            Format.Load(Settings, Path);

            Assert.AreEqual(true, _boolValue);
            Assert.AreEqual(5, _intValue);
            Assert.AreEqual(5.3453F, _floatValue);
            Assert.AreEqual("Test", _stringValue);

            Assert.AreEqual(Expected, File.ReadAllText(Path));
        }

        [Test]
        public void SaveLoads_Test()
        {
            _boolValue = true;
            _intValue = 5;
            _floatValue = 5.3453F;
            _stringValue = "Test";

            Format.Save(Settings, Path);
            Format.Load(Settings, Path);

            Assert.AreEqual(Expected, File.ReadAllText(Path));
        }

        [Test]
        public void Save_Test()
        {
            _boolValue = true;
            _intValue = 5;
            _floatValue = 5.3453F;
            _stringValue = "Test";

            Format.Save(Settings, Path);

            Assert.AreEqual(Expected, File.ReadAllText(Path));
        }

        [Test]
        public void Load_Test()
        {
            File.WriteAllText(Path, Expected);

            Format.Load(Settings, Path);

            Assert.AreEqual(true, _boolValue);
            Assert.AreEqual(5, _intValue);
            Assert.AreEqual(5.3453F, _floatValue);
            Assert.AreEqual("Test", _stringValue);
        }
    }
}