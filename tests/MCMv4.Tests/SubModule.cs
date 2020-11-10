using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.Ref;
using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Definitions;

using System;
using System.IO;

using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;

using Path = System.IO.Path;

namespace MCMv4.Tests
{
    public sealed class SubModule : MBSubModuleBase
    {
        private bool _boolValue;
        private int _intValue;
        private float _floatValue;
        private string _stringValue = string.Empty;

        /// <summary>
        /// End initialization
        /// </summary>
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            var builder = BaseSettingsBuilder.Create("Testing_Global_v4", "MCMv4 Testing Fluent Settings")!
                .SetFormat("xml")
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
                .CreatePreset("Test", presetBuilder => presetBuilder
                    .SetPropertyValue("prop_1", true)
                    .SetPropertyValue("prop_2", 2)
                    .SetPropertyValue("prop_3", 1.5F)
                    .SetPropertyValue("prop_4", "HueHueHue"));

            var globalSettings = builder.BuildAsGlobal();
            globalSettings.Register();
            //globalSettings.Unregister();

            //var perSaveSettings = builder.BuildAsPerSave();
            //perSaveSettings.Register();
            //perSaveSettings.Unregister();

            var path = Path.Combine(Utilities.GetBasePath(), "Modules", "Bannerlord.MBOptionScreen.Tests", "ExternalSettingsTest.xml");
            if (!File.Exists(path))
                throw new Exception("Settings does not exist in '/Modules' folder");
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var externalSettings = ExternalGlobalSettings.CreateFromXmlStream(fs, AssignRefDelegate);
            externalSettings?.Register();
        }

        private static IRef AssignRefDelegate(IPropertyDefinitionBase propertyDefinition) => propertyDefinition switch
        {
            IPropertyDefinitionBool _ => new ProxyRef<bool>(() => false, null),
            IPropertyDefinitionDropdown _ => new ProxyRef<bool>(() => false, null),
            IPropertyDefinitionWithMinMax _ => new ProxyRef<int>(() => 0, null),
            IPropertyDefinitionText _ => new ProxyRef<string>(() => string.Empty, null),
            _ => new ProxyRef<object?>(() => null, null)
        };
    }
}