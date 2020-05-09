using MCM.Abstractions.Settings.Models;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings
{
    public sealed class EmptyGlobalSettings : GlobalSettings<EmptyGlobalSettings>
    {
        public override string Id => "empty_v1";
        public override string DisplayName => $"Using Options Settings from v1 or v2!";
        public override string FolderName => "";
        public override string Format => "memory";

        protected override IEnumerable<SettingsPropertyGroupDefinition> GetUnsortedSettingPropertyGroups() => new List<SettingsPropertyGroupDefinition>();
    }
}