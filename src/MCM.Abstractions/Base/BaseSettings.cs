using HarmonyLib.BUTR.Extensions;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace MCM.Abstractions.Base
{
    /// <summary>
    /// Base model for MCM settings
    /// </summary>
    public abstract class BaseSettings : INotifyPropertyChanged
    {
        private delegate BaseSettings SettingsCtor();
        private static readonly ConcurrentDictionary<Type, SettingsCtor> _cachedConstructors = new();

        public const string SaveTriggered = "SAVE_TRIGGERED";
        public const string DefaultPresetId = "default";
        public const string DefaultPresetName = "{=BaseSettings_Default}Default";

        /// <inheritdoc/>
        public virtual event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Unique identifier used to save to file. Make sure this is unique to your mod.
        /// </summary>
        public abstract string Id { get; }
        /// <summary>
        /// The display name of the setting in the settings menu.
        /// </summary>
        public abstract string DisplayName { get; }
        public virtual string FolderName { get; } = string.Empty;
        /// <summary>
        /// If you want this settings file stored inside a subfolder, set this to the name of the subfolder.
        /// </summary>
        public virtual string SubFolder => string.Empty;
        public virtual string FormatType => "none";
        public virtual string DiscoveryType => "none";
        public virtual int UIVersion => 1;
        public virtual char SubGroupDelimiter => '/';

        public virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public virtual BaseSettings CreateNew()
        {
            var type = GetType();
            var constructor = _cachedConstructors.GetOrAdd(type, static t => AccessTools2.GetConstructorDelegate<SettingsCtor>(t) ??
                                                                             (SettingsCtor) (() => (BaseSettings) FormatterServices.GetUninitializedObject(t)));
            return constructor();
        }

        public virtual BaseSettings CopyAsNew()
        {
            var newSettings = CreateNew();
            SettingsUtils.OverrideSettings(newSettings, this);
            return newSettings;
        }

        public virtual IEnumerable<ISettingsPreset> GetBuiltInPresets()
        {
            yield return new MemorySettingsPreset(Id, DefaultPresetId, DefaultPresetName, CreateNew);
        }
    }
}