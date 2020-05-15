using MCM.Abstractions.Settings.Models;

using System.Collections.Generic;

namespace MCM.Abstractions.Settings
{
    public sealed class EmptyPerCharacterSettings : PerCharacterSettings<EmptyPerCharacterSettings>
    {
        public override string Id => "empty_perchar_v1";
        public override string DisplayName => "Empty PerCharacter Settings";
        public override string Format => "memory";

        protected override IEnumerable<SettingsPropertyGroupDefinition> GetUnsortedSettingPropertyGroups() => new List<SettingsPropertyGroupDefinition>();
    }
}