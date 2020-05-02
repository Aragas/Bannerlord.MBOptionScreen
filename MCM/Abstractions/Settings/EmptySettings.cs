using MCM.Abstractions.Settings.Definitions;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings
{
    public sealed class EmptySettings : SettingsBase<EmptySettings>
    {
        public override string Id => "empty_v1";
        public override string ModName => $"Using Options Settings from v1 or v2!";
        public override string ModuleFolderName => "";
        public override string Format => "memory";

        protected override IEnumerable<SettingsPropertyGroupDefinition> GetUnsortedSettingPropertyGroups() => new List<SettingsPropertyGroupDefinition>();
    }
}