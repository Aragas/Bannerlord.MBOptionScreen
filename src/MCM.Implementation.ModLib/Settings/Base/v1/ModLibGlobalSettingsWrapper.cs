using HarmonyLib;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.Settings.Base;
using MCM.Utils;

using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MCM.Implementation.ModLib.Settings.Base.v1
{
    [Version("e1.0.0",  2)]
    [Version("e1.0.1",  2)]
    [Version("e1.0.2",  2)]
    [Version("e1.0.3",  2)]
    [Version("e1.0.4",  2)]
    [Version("e1.0.5",  2)]
    [Version("e1.0.6",  2)]
    [Version("e1.0.7",  2)]
    [Version("e1.0.8",  2)]
    [Version("e1.0.9",  2)]
    [Version("e1.0.10", 2)]
    [Version("e1.0.11", 2)]
    [Version("e1.1.0",  2)]
    [Version("e1.2.0",  2)]
    [Version("e1.2.1",  2)]
    [Version("e1.3.0",  2)]
    [Version("e1.3.1",  2)]
    [Version("e1.4.0",  2)]
    [Version("e1.4.1",  2)]
    public class ModLibGlobalSettingsWrapper : BaseModLibGlobalSettingsWrapper
    {
        private PropertyInfo? IDProperty { get; }
        private PropertyInfo? ModuleFolderNameProperty { get; }
        private PropertyInfo? ModNameProperty { get; }
        private PropertyInfo? SubFolderProperty { get; }

        public override string Id => IDProperty?.GetValue(Object) as string ?? "ERROR";
        public override string FolderName => ModuleFolderNameProperty?.GetValue(Object) as string ?? string.Empty;
        public override string DisplayName => ModNameProperty?.GetValue(Object) as string ?? "ERROR";
        public override int UIVersion => 1;
        public override string SubFolder => SubFolderProperty?.GetValue(Object) as string ?? string.Empty;
        protected override char SubGroupDelimiter => '/';
        public override string Format => "json";
        public override event PropertyChangedEventHandler? PropertyChanged { add { } remove { } }

        public ModLibGlobalSettingsWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();
            IDProperty = AccessTools.Property(type, "ID");
            ModuleFolderNameProperty = AccessTools.Property(type, "ModuleFolderName");
            ModNameProperty = AccessTools.Property(type, "ModName");
            SubFolderProperty = AccessTools.Property(type, "SubFolder");

            IsCorrect = ReflectionUtils.ImplementsOrImplementsEquivalent(type, "ModLib.SettingsBase") &&
                        IDProperty != null && ModuleFolderNameProperty != null &&
                        ModNameProperty != null && SubFolderProperty != null;
        }

        internal void UpdateReference(object @object) => Object = @object;

        public override void OnPropertyChanged([CallerMemberName] string? propertyName = null) { }

        protected override BaseSettings CreateNew() => new ModLibGlobalSettingsWrapper(Activator.CreateInstance(Object.GetType()));
    }
}