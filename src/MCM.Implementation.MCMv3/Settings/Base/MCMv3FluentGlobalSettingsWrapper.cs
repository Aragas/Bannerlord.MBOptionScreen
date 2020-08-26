extern alias v3;
extern alias v4;

using HarmonyLib;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using v4::MCM.Abstractions.Settings.Base.Global;
using v4::MCM.Abstractions.Settings.Models.Wrapper;

using LegacySettingsPropertyGroupDefinition = v3::MCM.Abstractions.Settings.Models.SettingsPropertyGroupDefinition;
using SettingsPropertyGroupDefinition = v4::MCM.Abstractions.Settings.Models.SettingsPropertyGroupDefinition;

namespace MCM.Implementation.MCMv3.Settings.Base
{
    public class MCMv3FluentGlobalSettingsWrapper : BaseFluentGlobalSettingsWrapper
    {
        private static string GetId(object @object) => AccessTools.Property(@object.GetType(), nameof(Id))?.GetValue(@object) as string ?? "ERROR";
        private static string GetDisplayName(object @object) => AccessTools.Property(@object.GetType(), nameof(DisplayName))?.GetValue(@object) as string ?? "ERROR";
        private static string GetFolderName(object @object) => AccessTools.Property(@object.GetType(), nameof(FolderName))?.GetValue(@object) as string ?? "ERROR";
        private static string GetSubFolder(object @object) => AccessTools.Property(@object.GetType(), nameof(SubFolder))?.GetValue(@object) as string ?? "ERROR";
        private static string GetFormat(object @object) => AccessTools.Property(@object.GetType(), nameof(Format))?.GetValue(@object) as string ?? "memory";
        private static int GetUIVersion(object @object) => AccessTools.Property(@object.GetType(), nameof(UIVersion))?.GetValue(@object) as int? ?? 1;
        private static char GetSubGroupDelimiter(object @object) => AccessTools.Property(@object.GetType(), nameof(SubGroupDelimiter))?.GetValue(@object) as char? ?? '/';
        private static IEnumerable<SettingsPropertyGroupDefinition>? GetSettingPropertyGroups(object @object)
        {
            var settingPropertyGroups = AccessTools.Property(@object.GetType(), "SettingPropertyGroups")?.GetValue(@object) as IEnumerable<LegacySettingsPropertyGroupDefinition>;
            settingPropertyGroups ??= Enumerable.Empty<LegacySettingsPropertyGroupDefinition>();
            return settingPropertyGroups.Select(x => new SettingsPropertyGroupDefinitionWrapper(x));
        }

        private MethodInfo? OnPropertyChangedMethod { get; }

        public new event PropertyChangedEventHandler? PropertyChanged
        {
            add { if (Object is INotifyPropertyChanged notifyPropertyChanged) notifyPropertyChanged.PropertyChanged += value; }
            remove { if (Object is INotifyPropertyChanged notifyPropertyChanged) notifyPropertyChanged.PropertyChanged -= value; }
        }

        public MCMv3FluentGlobalSettingsWrapper(object @object)
            : base(@object, GetId(@object), GetDisplayName(@object), GetFolderName(@object), GetSubFolder(@object), GetFormat(@object),
                  GetUIVersion(@object), GetSubGroupDelimiter(@object), null, GetSettingPropertyGroups(@object) ?? Enumerable.Empty<SettingsPropertyGroupDefinition>())
        {
            var type = @object.GetType();
            OnPropertyChangedMethod = AccessTools.Method(type, "OnPropertyChanged");
        }

        public override void OnPropertyChanged([CallerMemberName] string? propertyName = null) => OnPropertyChangedMethod?.Invoke(Object, new object[] { propertyName! });
    }
}