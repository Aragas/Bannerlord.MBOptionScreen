using MBOptionScreen.Settings;

using System.Collections.Generic;

namespace MBOptionScreen
{
    internal sealed class StubSettings : SettingsBase<StubSettings>
    {
        public override string Id { get; set; } = "Stub_v2";
        public override string ModName => $"Using Options Settings from v1!";
        public override string ModuleFolderName => "";

        public override List<SettingPropertyGroupDefinition> GetSettingPropertyGroups() => new List<SettingPropertyGroupDefinition>();
    }
}