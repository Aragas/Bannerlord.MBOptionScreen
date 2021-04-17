﻿extern alias v1;

using HarmonyLib.BUTR.Extensions;

using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Base.Global;

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using LegacyBaseSettings = v1::ModLib.SettingsBase;

namespace MCM.Adapter.ModLib.Settings.Base.v1
{
    internal sealed class ModLibGlobalSettingsWrapper : BaseGlobalSettingsWrapper
    {
        private delegate string GetIdDelegate();
        private delegate string GetFolderNameDelegate();
        private delegate string GetDisplayNameDelegate();
        private delegate string GetSubFolderDelegate();

        private readonly GetIdDelegate? _getIdDelegate;
        private readonly GetFolderNameDelegate? _getFolderNameDelegate;
        private readonly GetDisplayNameDelegate? _getDisplayNameDelegate;
        private readonly GetSubFolderDelegate? _getSubFolderDelegate;

        public override string Id => _getIdDelegate?.Invoke() ?? "ERROR";
        public override string FolderName => _getFolderNameDelegate?.Invoke() ?? string.Empty;
        public override string DisplayName => _getDisplayNameDelegate?.Invoke() ?? "ERROR";
        public override int UIVersion => 1;
        public override string SubFolder => _getSubFolderDelegate?.Invoke() ?? string.Empty;
        public override char SubGroupDelimiter => '/';
        public override string FormatType => "memory";
        public override string DiscoveryType => "modlib_v1_attributes";
        public override event PropertyChangedEventHandler? PropertyChanged { add { } remove { } }

        public ModLibGlobalSettingsWrapper(object @object) : base(@object)
        {
            var type = @object.GetType();

            _getIdDelegate = AccessTools2.GetPropertyGetterDelegate<GetIdDelegate>(@object, type, nameof(LegacyBaseSettings.ID));
            _getFolderNameDelegate = AccessTools2.GetPropertyGetterDelegate<GetFolderNameDelegate>(@object, type, nameof(LegacyBaseSettings.ModuleFolderName));
            _getDisplayNameDelegate = AccessTools2.GetPropertyGetterDelegate<GetDisplayNameDelegate>(@object, type, nameof(LegacyBaseSettings.ModName));
            _getSubFolderDelegate = AccessTools2.GetPropertyGetterDelegate<GetSubFolderDelegate>(@object, type, nameof(LegacyBaseSettings.SubFolder));
        }

        internal void UpdateReference(object @object) => Object = @object;

        public override void OnPropertyChanged([CallerMemberName] string? propertyName = null) { }

        protected override BaseSettings CreateNew() => new ModLibGlobalSettingsWrapper(Activator.CreateInstance(Object.GetType())!);
    }
}