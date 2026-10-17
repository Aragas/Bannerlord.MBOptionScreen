## Overview
A mod can declare its settings in its `SubModule.xml` and MCM shows them in the options screen. The mod does not reference MCM, does not ship any MCM code and does not need MCM to run: the `SubModule.xml` is both the declaration and the store.

MCM reads the `<Settings>` blocks placed directly under `<Module>` of every loaded module and registers them as Global settings. Every property carries its `Default`; when the user changes something, MCM writes the new value back into the same file as a `Value` attribute next to it, keeping everything else in the file as it is:
```xml
<Bool Id="Enabled" DisplayName="Enabled" Default="true" Value="false" />
```
The mod reads `Value` when present and `Default` otherwise, and can write `Value` itself the same way when it changes a setting in code. Nothing is written to `Configs/ModSettings` for these settings. Presets saved by the user through MCM still go there, like for every other settings.

Because the module folder is the store, updating the mod replaces the file and the user's values with it, and on a platform where the module folder is not writable the values changed in the options screen apply for the running session only.

## Declaration
```xml
<Module>
  <Id value="MyMod" />
  <Name value="My Mod" />
  ...
  <Settings>
    <Group Name="{=MyMod_General}General" Order="1">
      <Bool Id="Enabled" DisplayName="{=MyMod_Enabled}Enabled" HintText="{=MyMod_EnabledHint}Turns the mod on." Default="true" RequireRestart="false" IsToggle="true" />
      <Integer Id="Retries" DisplayName="Retries" MinValue="0" MaxValue="10" Default="3" />
      <FloatingInteger Id="Scale" DisplayName="Scale" MinValue="0.5" MaxValue="2" Default="1" ValueFormat="0.00" />
      <Text Id="Prefix" DisplayName="Prefix" Default="[Mod]" />
      <Dropdown Id="Quality" DisplayName="Quality" Default="1">
        <Option Value="{=MyMod_Low}Low" />
        <Option Value="{=MyMod_High}High" />
      </Dropdown>
      <Group Name="{=MyMod_Advanced}Advanced">
        <Bool Id="Verbose" DisplayName="Verbose" Default="false" />
      </Group>
    </Group>
    <Preset Id="quiet" DisplayName="{=MyMod_Quiet}Quiet">
      <Property Id="Verbose" Value="false" />
      <Property Id="Quality" Value="0" />
    </Preset>
  </Settings>
</Module>
```

`<Settings>`
* `Id`: unique across all mods, used to look the settings up. Defaults to the module id; set it when a module declares more than one block (a second block with the same id is ignored).
* `DisplayName`: the name in the mod list. Defaults to the module name.
* `UIVersion`, `SubGroupDelimiter`: as in code, defaults `1` and `/`.

`<Group>` has `Name` and `Order`. Nested groups are sub groups. Properties placed directly under `<Settings>` go to the `Misc` group.

Property elements share `Id` (required), `DisplayName` (defaults to `Id`), `HintText`, `Order` and `RequireRestart` (default `true`). `Default` is the value that ships; `Value` is written by MCM or the mod and should not be in the file the mod ships.
* `Bool`: `IsToggle` makes it the group's toggle.
* `Integer`, `FloatingInteger`: `MinValue`, `MaxValue`, `ValueFormat`. `Default` falls back to `MinValue`.
* `Text`.
* `Dropdown`: one `<Option Value="" />` per entry, `Default` and `Value` are option indices.

`<Preset>` has `Id` and `DisplayName`; each `<Property Id="" Value="" />` is written the way `Default` is written. The declared defaults are always available as the `Default` preset. Buttons cannot be declared, they need code.

Any `{=id}text` value is localized through the game's string tables like every other MCM text. Numbers use the invariant culture, booleans are `true` and `false`.

## Reading and writing the values in the mod
Load your `SubModule.xml`, find the block and take each property's `Value` attribute, falling back to `Default`. To change a setting from code set the `Value` attribute and save the document; load it with `PreserveWhitespace` and write the bytes in place so comments, formatting and the links a mod manager deployed survive. Settings with `RequireRestart="false"` can be re-read when the file's write time changes, which is how a mod picks up changes made in the options screen without restarting. [UIExtenderEx](https://github.com/BUTR/Bannerlord.UIExtenderEx/blob/dev/src/Bannerlord.UIExtenderEx/Settings/SettingsSubModuleXml.cs) does exactly this.

Do not use a settings `Id` that a code based settings class of yours already registers, the options screen would show both.

## Who registers the block
With the standalone module (`Bannerlord.MBOptionScreen`) loaded, it registers every module's declarations and copies of MCM embedded in mods leave them alone. Without the standalone there is no options screen; each embedded copy then registers all of them so a mod that ships MCM can still access its settings through `BaseSettingsProvider.Instance.GetSettings(id)`.

A standalone XML file with `<Settings>` as its root is read and written the same way through `ExternalGlobalSettings.CreateFromFile`. There is no module to take defaults from, so such a file needs `Id`.
