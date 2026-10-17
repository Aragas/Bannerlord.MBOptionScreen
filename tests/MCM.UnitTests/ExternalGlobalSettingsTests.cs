extern alias v5;

using NUnit.Framework;

using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

using v5::MCM.Abstractions;
using v5::MCM.Abstractions.Base;
using v5::MCM.Abstractions.Base.Global;
using v5::MCM.Abstractions.Xml;
using v5::MCM.Common;

namespace MCM.Tests
{
    public class ExternalGlobalSettingsTests : BaseTests
    {
        private const string SubModuleXml = """
<?xml version="1.0" encoding="UTF-8"?>
<Module>
  <Id value="TestModule" />
  <Name value="Test Module" />
  <SubModules>
    <SubModule>
      <Name value="Test" />
      <DLLName value="Test.dll" />
      <SubModuleClassType value="Test.SubModule" />
      <Tags />
    </SubModule>
  </SubModules>
  <!-- keep me -->
  <Settings UIVersion="2">
    <Bool Id="Ungrouped" DisplayName="Ungrouped" Default="true" />
    <Group Name="{=TestModule_General}General" Order="1">
      <Bool Id="Enabled" DisplayName="{=TestModule_Enabled}Enabled" HintText="Hint" Default="true" Value="false" RequireRestart="false" IsToggle="true" />
      <Integer Id="Retries" DisplayName="Retries" MinValue="0" MaxValue="10" Default="3" Order="2" />
      <FloatingInteger Id="Scale" DisplayName="Scale" MinValue="0.5" MaxValue="2" Default="1.5" Value="0.75" ValueFormat="0.0" />
      <Text Id="Prefix" DisplayName="Prefix" Default="[Mod]" />
      <Dropdown Id="Quality" DisplayName="Quality" Default="1">
        <Option Value="Low" />
        <Option Value="High" />
      </Dropdown>
      <Group Name="Advanced">
        <Bool Id="Verbose" DisplayName="Verbose" Default="false" />
      </Group>
      <Unknown Id="Ignored" Default="true" />
      <Bool DisplayName="No id, ignored" Default="true" />
    </Group>
    <Preset Id="quiet" DisplayName="Quiet">
      <Property Id="Verbose" Value="false" />
      <Property Id="Retries" Value="0" />
      <Property Id="Quality" Value="0" />
      <Property Id="Missing" Value="1" />
    </Preset>
  </Settings>
  <Settings DisplayName="Same id as the first block, ignored" />
  <Settings Id="TestModule_extra" DisplayName="Extra"><Bool Id="Extra" Default="true" /></Settings>
</Module>
""";

        private static XmlDocument Load(string xml)
        {
            var document = new XmlDocument { PreserveWhitespace = true };
            document.LoadXml(xml);
            return document;
        }

        private static ISettingsPropertyDefinition Property(BaseSettings settings, string id) =>
            settings.GetAllSettingPropertyDefinitions().Single(x => x.Id == id);

        private static Dropdown<string> Dropdown(BaseSettings settings, string id) =>
            (Dropdown<string>) Property(settings, id).PropertyReference.Value!;

        [Test]
        public void Model_ReadsDeclaration()
        {
            var models = SettingsXmlModel.FromDocument(Load(SubModuleXml)).ToList();

            Assert.AreEqual(2, models.Count, "a block resolving to an id already seen is skipped");
            var model = models[0];
            Assert.AreEqual("TestModule", model.Id, "defaults to the module id");
            Assert.AreEqual("Test Module", model.DisplayName, "defaults to the module name");
            Assert.AreEqual(2, model.UIVersion);
            Assert.AreEqual("/", model.SubGroupDelimiter);

            Assert.AreEqual("TestModule_extra", models[1].Id);
            Assert.AreEqual("Extra", models[1].DisplayName);

            Assert.AreEqual(1, model.Properties.Count);
            Assert.AreEqual(1, model.Groups.Count);
            Assert.AreEqual(1, model.Presets.Count);

            var general = model.Groups[0];
            Assert.AreEqual(1, general.GroupOrder);
            Assert.AreEqual(5, general.Properties.Count, "unknown elements and elements without Id are skipped");
            Assert.AreEqual(1, general.Groups.Count);

            var enabled = (PropertyBoolXmlModel) general.Properties[0];
            Assert.IsTrue(enabled.DefaultValue);
            Assert.IsFalse(enabled.Value, "a saved Value wins over Default");
            Assert.IsTrue(enabled.IsToggle);
            Assert.IsFalse(enabled.RequireRestart);
            Assert.AreEqual("Hint", enabled.HintText);

            var retries = (PropertyIntegerXmlModel) general.Properties[1];
            Assert.AreEqual(3, retries.DefaultValue);
            Assert.AreEqual(3, retries.Value, "without a saved Value the Default applies");
            Assert.AreEqual(10, retries.MaxValue);
            Assert.AreEqual(2, retries.Order);
            Assert.IsTrue(retries.RequireRestart);

            var scale = (PropertyFloatingIntegerXmlModel) general.Properties[2];
            Assert.AreEqual(1.5M, scale.DefaultValue);
            Assert.AreEqual(0.75M, scale.Value);
            Assert.AreEqual(0.5M, scale.MinValue);
            Assert.AreEqual("0.0", scale.ValueFormat);

            var quality = (PropertyDropdownXmlModel) general.Properties[4];
            Assert.AreEqual(1, quality.DefaultSelectedIndex);
            Assert.AreEqual(1, quality.SelectedIndex);
            CollectionAssert.AreEqual(new[] { "Low", "High" }, quality.Values);

            Assert.AreEqual(7, model.GetAllProperties().Count());
        }

        [Test]
        public void Model_StandaloneFile_UsesRoot()
        {
            var models = SettingsXmlModel.FromDocument(Load("""<Settings Id="Root_v1"><Bool Id="A" Default="true" /></Settings>""")).ToList();

            Assert.AreEqual(1, models.Count);
            Assert.AreEqual("Root_v1", models[0].Id);
            Assert.AreEqual("Root_v1", models[0].DisplayName, "display name falls back to the id");
            Assert.AreEqual("A", models[0].Properties[0].DisplayName, "display name falls back to the id");

            Assert.IsEmpty(SettingsXmlModel.FromDocument(Load("""<Settings><Bool Id="A" Default="true" /></Settings>""")), "a standalone file needs an Id");
        }

        [Test]
        public void Settings_HaveGroupsValuesAndTypes()
        {
            var settings = ExternalGlobalSettings.CreateFromDocument(Load(SubModuleXml), "SubModule.xml").First();

            Assert.AreEqual("TestModule", settings.Id);
            Assert.AreEqual("SubModule.xml", settings.FilePath);
            Assert.AreEqual("fluent", settings.DiscoveryType);
            Assert.AreEqual(ExternalGlobalSettings.DeclarationFormatType, settings.FormatType);

            var groups = settings.GetSettingPropertyGroups();
            var general = groups.Single(x => x.GroupNameRaw == "{=TestModule_General}General");
            Assert.AreEqual(1, general.Order);
            Assert.AreEqual(5, general.SettingProperties.Count());
            Assert.AreEqual("Advanced", general.SubGroups.Single().GroupNameRaw, "nested group is a sub group");
            Assert.AreEqual(1, general.SubGroups.Single().SettingProperties.Count());
            Assert.AreEqual(1, groups.Single(x => x.GroupNameRaw == SettingsPropertyGroupDefinition.DefaultGroupName).SettingProperties.Count(), "ungrouped property is in the default group");

            Assert.AreEqual(SettingType.Bool, Property(settings, "Enabled").SettingType);
            Assert.AreEqual(false, Property(settings, "Enabled").PropertyReference.Value, "starts at the saved value");
            Assert.IsTrue(Property(settings, "Enabled").IsToggle);
            Assert.IsFalse(Property(settings, "Enabled").RequireRestart);

            Assert.AreEqual(SettingType.Int, Property(settings, "Retries").SettingType);
            Assert.AreEqual(3, Property(settings, "Retries").PropertyReference.Value);
            Assert.AreEqual(10, Property(settings, "Retries").MaxValue);

            Assert.AreEqual(SettingType.Float, Property(settings, "Scale").SettingType);
            Assert.AreEqual(0.75F, Property(settings, "Scale").PropertyReference.Value);
            Assert.AreEqual("0.0", Property(settings, "Scale").ValueFormat);

            Assert.AreEqual(SettingType.String, Property(settings, "Prefix").SettingType);
            Assert.AreEqual("[Mod]", Property(settings, "Prefix").PropertyReference.Value);

            Assert.AreEqual(SettingType.Dropdown, Property(settings, "Quality").SettingType);
            Assert.AreEqual(1, Dropdown(settings, "Quality").SelectedIndex);
            Assert.AreEqual("High", Dropdown(settings, "Quality").SelectedValue);
        }

        [Test]
        public void Settings_Presets()
        {
            var settings = ExternalGlobalSettings.CreateFromDocument(Load(SubModuleXml), "SubModule.xml").First();

            var presets = settings.GetBuiltInPresets().ToList();
            CollectionAssert.AreEqual(new[] { BaseSettings.DefaultPresetId, "quiet" }, presets.Select(x => x.Id));

            var quiet = presets[1].LoadPreset();
            Assert.AreEqual(false, Property(quiet, "Verbose").PropertyReference.Value);
            Assert.AreEqual(0, Property(quiet, "Retries").PropertyReference.Value);
            Assert.AreEqual(0, Dropdown(quiet, "Quality").SelectedIndex);
            Assert.AreEqual(false, Property(quiet, "Enabled").PropertyReference.Value, "untouched properties keep their value");

            var defaults = presets[0].LoadPreset();
            Assert.AreEqual(true, Property(defaults, "Enabled").PropertyReference.Value, "the default preset holds Default, not the saved Value");
            Assert.AreEqual(1.5F, Property(defaults, "Scale").PropertyReference.Value);

            Property(settings, "Retries").PropertyReference.Value = 7;
            Dropdown(settings, "Quality").SelectedIndex = 0;

            defaults = presets[0].LoadPreset();
            Assert.AreEqual(3, Property(defaults, "Retries").PropertyReference.Value);
            Assert.AreEqual(1, Dropdown(defaults, "Quality").SelectedIndex, "the default preset is not affected by later changes");

            SettingsUtils.ResetSettings(settings);
            Assert.AreEqual(3, Property(settings, "Retries").PropertyReference.Value);
            Assert.AreEqual(true, Property(settings, "Enabled").PropertyReference.Value);
            Assert.AreEqual(1.5F, Property(settings, "Scale").PropertyReference.Value);
            Assert.AreEqual(1, Dropdown(settings, "Quality").SelectedIndex);
        }

        [Test]
        public void Settings_SaveToFile_WritesValueAttributes()
        {
            var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, $"{nameof(Settings_SaveToFile_WritesValueAttributes)}.xml");
            File.WriteAllText(path, SubModuleXml);
            try
            {
                var settings = ExternalGlobalSettings.CreateFromFile(path).First();
                Property(settings, "Enabled").PropertyReference.Value = true;
                Property(settings, "Retries").PropertyReference.Value = 7;
                Property(settings, "Scale").PropertyReference.Value = 1.25F;
                Property(settings, "Prefix").PropertyReference.Value = "x";
                Dropdown(settings, "Quality").SelectedIndex = 0;

                Assert.IsTrue(settings.SaveToFile());

                var xml = File.ReadAllText(path);
                StringAssert.Contains("<!-- keep me -->", xml, "comments survive");
                StringAssert.Contains("""<Bool Id="Enabled" DisplayName="{=TestModule_Enabled}Enabled" HintText="Hint" Default="true" Value="true" RequireRestart="false" IsToggle="true" />""", xml, "the existing Value is replaced in place");
                StringAssert.Contains("""<Integer Id="Retries" DisplayName="Retries" MinValue="0" MaxValue="10" Default="3" Order="2" Value="7" />""", xml, "a new Value is appended");
                StringAssert.Contains("""Default="1.5" Value="1.25" ValueFormat="0.0" />""", xml);
                StringAssert.Contains("""<Text Id="Prefix" DisplayName="Prefix" Default="[Mod]" Value="x" />""", xml);
                StringAssert.Contains("""<Dropdown Id="Quality" DisplayName="Quality" Default="1" Value="0">""", xml, "a dropdown is saved as the option index");
                StringAssert.Contains("""<Bool Id="Verbose" DisplayName="Verbose" Default="false" Value="false" />""", xml);
                StringAssert.Contains("""<Settings Id="TestModule_extra" DisplayName="Extra"><Bool Id="Extra" Default="true" /></Settings>""", xml, "other blocks are untouched");

                var reloaded = ExternalGlobalSettings.CreateFromFile(path).First();
                Assert.AreEqual(true, Property(reloaded, "Enabled").PropertyReference.Value);
                Assert.AreEqual(7, Property(reloaded, "Retries").PropertyReference.Value);
                Assert.AreEqual(1.25F, Property(reloaded, "Scale").PropertyReference.Value);
                Assert.AreEqual("x", Property(reloaded, "Prefix").PropertyReference.Value);
                Assert.AreEqual(0, Dropdown(reloaded, "Quality").SelectedIndex);
                Assert.AreEqual(3, Property(reloaded.GetBuiltInPresets().First().LoadPreset(), "Retries").PropertyReference.Value, "Default is untouched");
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Test]
        public void Settings_SaveToFile_FailsWithoutTheBlock()
        {
            var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, $"{nameof(Settings_SaveToFile_FailsWithoutTheBlock)}.xml");
            File.WriteAllText(path, SubModuleXml);
            try
            {
                var settings = ExternalGlobalSettings.CreateFromFile(path).First();
                File.WriteAllText(path, """<Module><Id value="TestModule" /></Module>""");
                Assert.IsFalse(settings.SaveToFile());

                File.Delete(path);
                Assert.IsFalse(settings.SaveToFile(), "a missing file is reported, not thrown");
            }
            finally
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        [TestCase("\n", false)]
        [TestCase("\r\n", false)]
        [TestCase("\n", true)]
        [TestCase("\r\n", true)]
        public void Settings_SaveToFile_ChangesOnlyTheValues(string newLine, bool bom)
        {
            var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, $"{nameof(Settings_SaveToFile_ChangesOnlyTheValues)}.xml");
            var xml = SubModuleXml.Replace("\r\n", "\n").Replace("\n", newLine);
            var encoding = new UTF8Encoding(bom);
            File.WriteAllBytes(path, encoding.GetPreamble().Concat(encoding.GetBytes(xml)).ToArray());
            try
            {
                var settings = ExternalGlobalSettings.CreateFromFile(path).First();
                Property(settings, "Retries").PropertyReference.Value = 7;
                Assert.IsTrue(settings.SaveToFile());

                var expected = xml;
                foreach (var property in settings.GetAllSettingPropertyDefinitions())
                {
                    var marker = $"Id=\"{property.Id}\"";
                    var start = expected.IndexOf(marker, System.StringComparison.Ordinal);
                    var end = expected.IndexOf('>', start);
                    var selfClosing = expected[end - 1] == '/';
                    var element = expected.Substring(start, end - start);
                    if (element.Contains(" Value=\"")) continue; // Enabled and Scale already have the value they are saved with
                    var insertAt = selfClosing ? end - 2 : end;
                    expected = expected.Insert(insertAt, $" Value=\"{ExternalGlobalSettings.FormatValue(property)}\"");
                }

                CollectionAssert.AreEqual(encoding.GetPreamble().Concat(encoding.GetBytes(expected)).ToArray(), File.ReadAllBytes(path),
                    "only the Value attributes change: the declaration, line endings and byte order mark are what the file had");
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Test]
        public void Settings_SaveToFile_KeepsTheMarkup()
        {
            const string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                               "<Module xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'\n" +
                               "        xsi:noNamespaceSchemaLocation=\"SubModule.xsd\" >\n" +
                               "  <Id value=\"Markup\" />\n" +
                               "  <SubModules><SubModule><Tags/></SubModule></SubModules>\n" +
                               "  <!-- keep -->\n" +
                               "  <Settings>\n" +
                               "\t<Bool Id='A' Default='false'/>\n" +
                               "\t<Integer Id=\"B\"\n" +
                               "\t         MinValue=\"0\" MaxValue=\"10\" Default=\"3\"></Integer>\n" +
                               "\t<Text Id=\"C\" Default=\"x\" Value='old' />\n" +
                               "  </Settings>\n" +
                               "</Module>\n";
            var expected = xml
                .Replace("<Bool Id='A' Default='false'/>", "<Bool Id='A' Default='false' Value=\"true\"/>")
                .Replace("Default=\"3\"></Integer>", "Default=\"3\" Value=\"7\"></Integer>")
                .Replace("Value='old'", "Value='it&apos;s \"q\" &amp; &lt;'");

            var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, $"{nameof(Settings_SaveToFile_KeepsTheMarkup)}.xml");
            File.WriteAllBytes(path, new UTF8Encoding(false).GetBytes(xml));
            try
            {
                var settings = ExternalGlobalSettings.CreateFromFile(path).Single();
                Property(settings, "A").PropertyReference.Value = true;
                Property(settings, "B").PropertyReference.Value = 7;
                Property(settings, "C").PropertyReference.Value = "it's \"q\" & <";
                Assert.IsTrue(settings.SaveToFile());

                Assert.AreEqual(expected, File.ReadAllText(path), "only the Value attributes change, in the quotes they already had");
                Assert.AreEqual("it's \"q\" & <", Property(ExternalGlobalSettings.CreateFromFile(path).Single(), "C").PropertyReference.Value);

                var writeTime = new System.DateTime(2000, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
                File.SetLastWriteTimeUtc(path, writeTime);
                Assert.IsTrue(settings.SaveToFile());
                Assert.AreEqual(writeTime, File.GetLastWriteTimeUtc(path), "nothing changed, nothing written: a mod watching the write time is not woken");
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Test]
        public void Settings_SaveToFile_RoundTripsFloats()
        {
            var path = Path.Combine(TestContext.CurrentContext.WorkDirectory, $"{nameof(Settings_SaveToFile_RoundTripsFloats)}.xml");
            File.WriteAllText(path, SubModuleXml);
            try
            {
                var settings = ExternalGlobalSettings.CreateFromFile(path).First();
                foreach (var value in new[] { 1E-05F, 0.1F, 1.2345679F, 123456.79F })
                {
                    Property(settings, "Scale").PropertyReference.Value = value;
                    Assert.IsTrue(settings.SaveToFile());
                    Assert.AreEqual(value, Property(ExternalGlobalSettings.CreateFromFile(path).First(), "Scale").PropertyReference.Value, $"'{ExternalGlobalSettings.FormatValue(Property(settings, "Scale"))}' reads back");
                }
            }
            finally
            {
                File.Delete(path);
            }
        }
    }
}
