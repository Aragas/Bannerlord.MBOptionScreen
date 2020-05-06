using MCM.Abstractions.Settings.Definitions;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings
{
    public sealed class EmptyPerCharacterSettings : PerCharacterSettings<EmptyPerCharacterSettings>
    {
        public override string Id => "empty_perchar_v1";
        public override string DisplayName => $"Using Options Settings from v1 or v2!";

        protected override IEnumerable<SettingsPropertyGroupDefinition> GetUnsortedSettingPropertyGroups() => new List<SettingsPropertyGroupDefinition>();
    }
}