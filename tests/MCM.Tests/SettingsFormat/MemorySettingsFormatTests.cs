extern alias v5;

using NUnit.Framework;

using System;
using System.IO;

using v5::MCM.Abstractions.FluentBuilder;
using v5::MCM.Abstractions.Ref;
using v5::MCM.Abstractions.Settings.Formats;

namespace MCM.Tests.SettingsFormat
{
    public class MemorySettingsFormatTests : BaseSettingsFormatTests
    {
        [OneTimeSetUp]
        public void OneTimeSetUp1()
        {
            Format = new MemorySettingsFormat();

            Settings = BaseSettingsBuilder.Create("Testing_Global_v1", "Testing Fluent Settings")!
                .SetFormat("memory")
                .SetFolderName(string.Empty)
                .SetSubFolder(string.Empty)
                .CreateGroup("Testing 1", groupBuilder => groupBuilder
                    .AddBool("prop_1", "Check Box", new ProxyRef<bool>(() => _boolValue, o => _boolValue = o), boolBuilder => boolBuilder
                        .SetHintText("Test")
                        .SetRequireRestart(false)))
                .CreateGroup("Testing 2", groupBuilder => groupBuilder
                    .AddInteger("prop_2", "Integer", 0, 10, new ProxyRef<int>(() => _intValue, o => _intValue = o), integerBuilder => integerBuilder
                         .SetHintText("Testing"))
                    .AddFloatingInteger("prop_3", "Floating Integer", 0, 10, new ProxyRef<float>(() => _floatValue, o => _floatValue = o), floatingBuilder => floatingBuilder
                         .SetRequireRestart(true)
                         .SetHintText("Test")))
                .CreateGroup("Testing 3", groupBuilder => groupBuilder
                    .AddText("prop_4", "Test", new ProxyRef<string>(() => _stringValue, o => _stringValue = o), null))
                .BuildAsGlobal();

            DirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Settings.FolderName, Settings.SubFolder);
            Filename = Settings.Id;
        }

        [Test]
        public void Serialize_Test()
        {
            Assert.AreEqual(false, _boolValue);
            Assert.AreEqual(0, _intValue);
            Assert.AreEqual(0F, _floatValue);
            Assert.AreEqual(string.Empty, _stringValue);

            Format.Save(Settings, DirectoryPath, Filename);
            Format.Load(Settings, DirectoryPath, Filename);

            Assert.AreEqual(false, _boolValue);
            Assert.AreEqual(0, _intValue);
            Assert.AreEqual(0F, _floatValue);
            Assert.AreEqual(string.Empty, _stringValue);


            _boolValue = true;
            _intValue = 5;
            _floatValue = 5.3453F;
            _stringValue = "Test";

            Format.Save(Settings, DirectoryPath, Filename);
            Format.Load(Settings, DirectoryPath, Filename);

            Assert.AreEqual(true, _boolValue);
            Assert.AreEqual(5, _intValue);
            Assert.AreEqual(5.3453F, _floatValue);
            Assert.AreEqual("Test", _stringValue);
        }

        [Test]
        public void SaveLoads_Test()
        {
            _boolValue = true;
            _intValue = 5;
            _floatValue = 5.3453F;
            _stringValue = "Test";

            Format.Save(Settings, DirectoryPath, Filename);
            Format.Load(Settings, DirectoryPath, Filename);
        }
    }
}