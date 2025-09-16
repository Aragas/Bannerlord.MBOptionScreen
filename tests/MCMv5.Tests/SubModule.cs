using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions.FluentBuilder;
using MCM.Common;

using System;

using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace MCMv5.Tests;

public sealed class SubModule : MBSubModuleBase
{
    private class DropdownContainer
    {
        public Dropdown<string> Dropdown { get; set; } = new Dropdown<string>(new[]
        {
            "One",
            "Two",
            "Three",
        }, 2);
    }

    private bool _boolValue;
    private int _intValue;
    private float _floatValue;
    private string _stringValue = string.Empty;

    private bool _boolValue2;
    private int _intValue2;
    private float _floatValue2;
    private string _stringValue2 = string.Empty;

    /// <summary>
    /// End initialization
    /// </summary>
    protected override void OnBeforeInitialModuleScreenSetAsRoot()
    {
        var storageDropdownValue = new Dropdown<string>(new[]
        {
            "One",
            "Two",
            "Three",
        }, 2);
        var storageDropdown = new StorageRef<Dropdown<string>>(storageDropdownValue);

        var propertyDropdownValue = new DropdownContainer();
        var propertyDropdown = new PropertyRef(SymbolExtensions2.GetPropertyInfo((DropdownContainer x) => x.Dropdown)!, propertyDropdownValue);

        var proxyDropdownValue = new Dropdown<string>(new[]
        {
            "One",
            "Two",
            "Three",
        }, 2);
        var proxyDropdown = new ProxyRef<Dropdown<string>>(() => proxyDropdownValue, o => proxyDropdownValue = o);

        var changingString = new StorageRef<string>(string.Empty);
        storageDropdownValue.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == "SelectedIndex")
            {
                if (sender is Dropdown<string> { SelectedIndex: 0 })
                    changingString.Value = "Selected Zero";
                if (sender is Dropdown<string> { SelectedIndex: 1 })
                    changingString.Value = "Selected One";
                if (sender is Dropdown<string> { SelectedIndex: 2 })
                    changingString.Value = "Selected Two ";
            }
        };

        var builder = BaseSettingsBuilder.Create("Testing_Global_v5", "MCMv5 Testing Fluent Settings")!
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
            .CreateGroup("Testing 4", groupBuilder => groupBuilder
                .AddButton("prop_5", "Test", new StorageRef((Action) (() => { })), "Test", null))
            .CreateGroup("Testing 5", groupBuilder => groupBuilder
                .AddDropdown("prop_6", "Test Storage Ref", storageDropdownValue.SelectedIndex, storageDropdown, null)
                .AddButton("prop_6_b", "Reset Storage Ref", new StorageRef((Action) (() => storageDropdownValue.SelectedIndex = 0)), "Reset Storage Ref", null)

                .AddDropdown("prop_7", "Test Property Ref", propertyDropdownValue.Dropdown.SelectedIndex, propertyDropdown, null)
                .AddButton("prop_7_b", "Reset Property Ref", new StorageRef((Action) (() => propertyDropdownValue.Dropdown.SelectedIndex = 0)), "Reset Property Ref", null)

                .AddDropdown("prop_8", "Test Proxy Ref", proxyDropdownValue.SelectedIndex, proxyDropdown, null)
                .AddButton("prop_8_b", "Reset Proxy Ref", new StorageRef((Action) (() => proxyDropdownValue.SelectedIndex = 0)), "Reset Proxy Ref", null)
                .AddText("prop_9", "Test", changingString, null)
            )
            .CreatePreset("test_v1", "Test", presetBuilder => presetBuilder
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
    }

    /// <inheritdoc />
    public override void OnAfterGameInitializationFinished(Game game, object starterObject)
    {
        var builder = BaseSettingsBuilder.Create("Testing_PerSave_v5", "MCMv5 Testing Fluent PerSave Settings")!
            .SetFormat("xml")
            .SetFolderName(string.Empty)
            .SetSubFolder(string.Empty)
            .CreateGroup("Testing 1", groupBuilder => groupBuilder
                .AddBool("prop_1", "Check Box", new ProxyRef<bool>(() => _boolValue2, o => _boolValue2 = o), boolBuilder => boolBuilder
                    .SetHintText("Test")
                    .SetRequireRestart(false)))
            .CreateGroup("Testing 2", groupBuilder => groupBuilder
                .AddInteger("prop_2", "Integer", 0, 10, new ProxyRef<int>(() => _intValue2, o => _intValue2 = o), integerBuilder => integerBuilder
                    .SetHintText("Testing"))
                .AddFloatingInteger("prop_3", "Floating Integer", 0, 10, new ProxyRef<float>(() => _floatValue2, o => _floatValue2 = o), floatingBuilder => floatingBuilder
                    .SetRequireRestart(true)
                    .SetHintText("Test")))
            .CreateGroup("Testing 3", groupBuilder => groupBuilder
                .AddText("prop_4", "Test", new ProxyRef<string>(() => _stringValue2, o => _stringValue2 = o), null))
            .CreateGroup("Testing 4", groupBuilder => groupBuilder
                .AddButton("prop_5", "Test2", new StorageRef((Action) (() => { })), "Test", null))
            .CreatePreset("test_v1", "Test", presetBuilder => presetBuilder
                .SetPropertyValue("prop_1", true)
                .SetPropertyValue("prop_2", 2)
                .SetPropertyValue("prop_3", 1.5F)
                .SetPropertyValue("prop_4", "HueHueHue"));

        var globalSettings = builder.BuildAsPerSave();
        globalSettings.Register();

        base.OnAfterGameInitializationFinished(game, starterObject);
    }
}