### Notable changes:
* MCMv4 depends on Bannerlord.Harmony, Bannerlord.ButterLib and Bannerlord.UIExtenderEx.
* Dropped MCM's dependency injection abstraction in favor of Microsoft.DependencyInjection that ButterLib provides.
* Dropped support for MBOv1/MBOv2/MCMv2
* Added support for MCMv3


* FluentAPI now supports Presets.
* PerCharacter settings are replaced by PerSave.
* BaseSettings.Format was renamed to BaseSettings.FormatType.
* Added new field BaseSettings.DiscoveryType that replaces BaseSettings.Discoverer.
* Removed BaseSettings.GetSettingPropertyGroups and BaseSettings.GetUnsortedSettingPropertyGroups. Use BaseSettingsExtensions.
* Fluent settings are not stored in the AppDomain dictionary anymore

* SettingPropertyGroupAttribute.IsMainToggle is mooved to SettingPropertyBoolAttribute.IsToggle

* Added a second json formatter `json2` that serializes the dropdown as an integer instead of a structure with SelectedIndex property

* Added a settings screen for ButterLib