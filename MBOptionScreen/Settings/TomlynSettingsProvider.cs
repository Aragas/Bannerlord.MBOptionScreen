#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using MBOptionScreen.Interfaces;
using MBOptionScreen.Settings;
using Tomlyn;
using Tomlyn.Syntax;

namespace MBOptionScreen.Settings {

  [PublicAPI]
  public class TomlynSettingsProvider : ISettingsProvider {

    private readonly string _defaultRootFolder = Path.Combine(Utilities.GetConfigsPath(), "ModSettings");

    private Dictionary<string, SettingsBase> LoadedSettings { get; } = new Dictionary<string, SettingsBase>();

    public List<ModSettingsDefinition> CreateModSettingsDefinitions => LoadedSettings.Keys
      .Select(id => new ModSettingsDefinition(id))
      .OrderByDescending(a => a.ModName)
      .ToList();

    public bool RegisterSettings(SettingsBase settingsInstance) {
      if (settingsInstance == null || LoadedSettings.ContainsKey(settingsInstance.Id))
        return false;

      LoadedSettings.Add(settingsInstance.Id, settingsInstance);
      var path = GetPath(settingsInstance);

      if (!File.Exists(path))
        return true;

      var bytes = File.ReadAllBytes(path);
      var toml = Toml.Parse(bytes, path);
      foreach (var group in settingsInstance.GetSettingPropertyGroups()) {
        var groupName = @group.GroupName;
        ITomlReader? reader = null;

        if (groupName == "Misc")
          reader = new TomlReader(toml);

        else {
          var table = toml.Tables.FirstOrDefault(t => t.Name.Key.ToString() == groupName);
          if (table != null)
            reader = new TomlTableReader(table);
        }

        if (reader == null)
          continue;

        foreach (var prop in @group.SettingProperties) {
          switch (prop.SettingType) {
            case SettingType.NONE: break;
            case SettingType.Bool:
              prop.Property.SetValue(settingsInstance, reader.Get<bool>(prop.SettingsId));
              break;
            case SettingType.Int:
              prop.Property.SetValue(settingsInstance, reader.Get<int>(prop.SettingsId));
              break;
            case SettingType.Float:
              prop.Property.SetValue(settingsInstance, reader.Get<float>(prop.SettingsId));
              break;
            case SettingType.String:
              prop.Property.SetValue(settingsInstance, reader.Get<string>(prop.SettingsId));
              break;
            case SettingType.Dropdown: throw new NotImplementedException();
            default: throw new ArgumentOutOfRangeException();
          }
        }
      }

      return true;
    }

    private string GetPath(SettingsBase settingsInstance)
      => GetPath(settingsInstance.ModuleFolderName, settingsInstance.SubFolder, settingsInstance.Id);

    private string GetPath(string modFolder, string subFolder, string id)
      => Path.Combine(_defaultRootFolder, modFolder, subFolder ?? "", $"{id}.txt");

    public SettingsBase? GetSettings(string id)
      => LoadedSettings.TryGetValue(id, out var result)
        ? result
        : null;

    public void SaveSettings(SettingsBase settingsInstance) {
      var path = GetPath(settingsInstance);

      var toml = new DocumentSyntax();
      foreach (var group in settingsInstance.GetSettingPropertyGroups()) {
        var table = new TableSyntax(group.GroupName);
        var entries = table.Items;
        foreach (var prop in group.SettingProperties) {
          switch (prop.SettingType) {
            case SettingType.NONE: break;
            case SettingType.Bool:
              entries.Add(prop.SettingsId, (bool) prop.Property.GetValue(settingsInstance));
              break;
            case SettingType.Int:
              entries.Add(prop.SettingsId, (int) prop.Property.GetValue(settingsInstance));
              break;
            case SettingType.Float:
              entries.Add(prop.SettingsId, (float) prop.Property.GetValue(settingsInstance));
              break;
            case SettingType.String:
              entries.Add(prop.SettingsId, (string) prop.Property.GetValue(settingsInstance));
              break;
            case SettingType.Dropdown: throw new NotImplementedException();
            default: throw new ArgumentOutOfRangeException();
          }
        }
      }

      using var sw = new StreamWriter(path, false, Encoding.UTF8, 65536) {NewLine = "\n"};
      foreach (var kv in toml.KeyValues)
        sw.WriteLine(kv.ToString().Trim('\n'));
      sw.WriteLine();
    }

    public bool OverrideSettings(SettingsBase newSettingsInstance) {
      if (newSettingsInstance == null || !LoadedSettings.ContainsKey(newSettingsInstance.Id))
        return false;

      LoadedSettings[newSettingsInstance.Id] = newSettingsInstance;
      SaveSettings(newSettingsInstance);
      return true;
    }

    public SettingsBase? ResetSettings(string id) {
      if (!LoadedSettings.TryGetValue(id, out var settingsInstance))
        return null;

      var path = GetPath(settingsInstance);
      if (File.Exists(path))
        File.WriteAllBytes(path, new byte[0]);

      return settingsInstance;
    }

  }

}