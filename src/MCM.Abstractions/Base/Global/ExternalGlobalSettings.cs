using MCM.Abstractions.FluentBuilder;
using MCM.Abstractions.Xml;
using MCM.Common;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

namespace MCM.Abstractions.Base.Global
{
    /// <summary>
    /// Global settings declared in XML (<see cref="SettingsXmlModel"/>) instead of code.
    /// MCM creates them for every <c>&lt;Settings&gt;</c> block found in the <c>SubModule.xml</c> of a loaded module,
    /// so a mod gets a settings page without referencing MCM. The declaring file is also the store: saving writes
    /// the current values back into it as <c>Value</c> attributes (<see cref="SaveToFile"/>), where the mod reads them.
    /// </summary>
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    sealed class ExternalGlobalSettings : FluentGlobalSettings
    {
        private sealed class PresetBuilder : ISettingsPresetBuilder
        {
            private readonly string _id;
            private readonly string _name;
            private readonly Dictionary<string, object?> _values = new();

            public PresetBuilder(string id, string name)
            {
                _id = id;
                _name = name;
            }

            public ISettingsPresetBuilder SetPropertyValue(string propertyName, object? value)
            {
                _values[propertyName] = value;
                return this;
            }

            public ISettingsPreset Build(BaseSettings settings) => new MemorySettingsPreset(settings.Id, _id, _name, () =>
            {
                var properties = settings.GetAllSettingPropertyDefinitions().ToList();
                foreach (var kv in _values)
                {
                    if (properties.Find(x => x.Id == kv.Key) is { } property)
                        property.PropertyReference.Value = CloneValue(kv.Value);
                }
                return settings;
            });
        }

        /// <summary>
        /// The format type of settings stored in their declaring XML file. No <see cref="ISettingsFormat"/> handles it,
        /// the container saves through <see cref="SaveToFile"/>.
        /// </summary>
        public const string DeclarationFormatType = "xml-declaration";

        /// <summary>
        /// The file the settings were declared in and are saved to: a standalone settings file or a module's <c>SubModule.xml</c>.
        /// </summary>
        public string FilePath { get; init; }

        /// <summary>
        /// Reads the first <c>&lt;Settings&gt;</c> block of an XML file. See <see cref="CreateFromFile"/>.
        /// </summary>
        public static ExternalGlobalSettings? CreateFromXmlFile(string filePath, PropertyChangedEventHandler? propertyChanged = null) =>
            CreateFromFile(filePath, propertyChanged).FirstOrDefault();

        /// <summary>
        /// Reads every <c>&lt;Settings&gt;</c> block of an XML file. The file is either a standalone settings file
        /// whose root is <c>&lt;Settings&gt;</c>, or a <c>SubModule.xml</c> whose <c>&lt;Module&gt;</c> root holds the blocks.
        /// </summary>
        public static IEnumerable<ExternalGlobalSettings> CreateFromFile(string filePath, PropertyChangedEventHandler? propertyChanged = null) =>
            CreateFromDocument(SettingsXmlWriter.Load(filePath), filePath, propertyChanged);

        /// <summary>
        /// Reads every <c>&lt;Settings&gt;</c> block of a loaded document. <paramref name="sourcePath"/> is kept as <see cref="FilePath"/>.
        /// </summary>
        public static IEnumerable<ExternalGlobalSettings> CreateFromDocument(XmlDocument document, string sourcePath, PropertyChangedEventHandler? propertyChanged = null) =>
            SettingsXmlModel.FromDocument(document).Select(model => Create(model, sourcePath, propertyChanged)).ToList();

        /// <summary>
        /// Reads one <c>&lt;Settings&gt;</c> element. Returns null when the element has no <c>Id</c>.
        /// </summary>
        public static ExternalGlobalSettings? CreateFromXml(XmlNode settingsNode, string sourcePath, PropertyChangedEventHandler? propertyChanged = null) =>
            SettingsXmlModel.FromXml(settingsNode) is { } model ? Create(model, sourcePath, propertyChanged) : null;

        /// <summary>
        /// Builds the settings from a parsed model. Properties start at their current value, nested groups become
        /// sub groups, the declared defaults become the default preset, and every <c>&lt;Preset&gt;</c> a built-in preset.
        /// </summary>
        public static ExternalGlobalSettings Create(SettingsXmlModel model, string sourcePath, PropertyChangedEventHandler? propertyChanged = null)
        {
            var delimiter = model.SubGroupDelimiterChar;

            var grouped = model.GetGroupedProperties().ToList();
            var properties = grouped
                .Select(x => new SettingsPropertyDefinition(SettingsUtils.GetPropertyDefinitionWrappers(x.Property), x.Group, CreateReference(x.Property), delimiter))
                .ToList();

            var presets = new List<ISettingsPresetBuilder>();

            var defaultPreset = new PresetBuilder(DefaultPresetId, DefaultPresetName);
            foreach (var (_, property) in grouped)
                defaultPreset.SetPropertyValue(property.Id, CreateDefaultValue(property));
            presets.Add(defaultPreset);

            foreach (var presetModel in model.Presets)
            {
                if (presetModel.Id == DefaultPresetId) continue;

                var preset = new PresetBuilder(presetModel.Id, presetModel.DisplayName);
                foreach (var kv in presetModel.Values)
                {
                    if (properties.Find(x => x.Id == kv.Key) is { } property && TryParseValue(property, kv.Value, out var value))
                        preset.SetPropertyValue(kv.Key, value);
                }
                presets.Add(preset);
            }

            return new ExternalGlobalSettings(
                model.Id,
                model.DisplayName,
                model.UIVersion,
                delimiter,
                propertyChanged,
                SettingsUtils.GetSettingsPropertyGroups(delimiter, properties),
                presets,
                sourcePath);
        }

        /// <summary>
        /// Writes the current values into <see cref="FilePath"/> as <c>Value</c> attributes. Returns false when the
        /// file cannot be read or written or no longer holds the block; the values then live for this session only.
        /// </summary>
        public bool SaveToFile()
        {
            try
            {
                var values = this.GetAllSettingPropertyDefinitions()
                    .Where(x => x.SettingType != SettingType.Button)
                    .Select(x => new KeyValuePair<string, string>(x.Id, FormatValue(x)));
                return SettingsXmlWriter.SetValues(FilePath, Id, values);
            }
            catch (Exception e) when (e is IOException or UnauthorizedAccessException or XmlException)
            {
                return false;
            }
        }

        /// <summary>
        /// A reference holding the current value of the property. Its type decides the <see cref="SettingType"/>.
        /// </summary>
        public static IRef CreateReference(PropertyBaseXmlModel model) => model switch
        {
            PropertyBoolXmlModel m => new StorageRef<bool>(m.Value),
            PropertyIntegerXmlModel m => new StorageRef<int>((int) m.Value),
            PropertyFloatingIntegerXmlModel m => new StorageRef<float>((float) m.Value),
            PropertyTextXmlModel m => new StorageRef<string>(m.Value),
            PropertyDropdownXmlModel m => new StorageRef<Dropdown<string>>(new Dropdown<string>(m.Values, m.SelectedIndex)),
            _ => throw new NotSupportedException($"Settings property '{model.Id}' has an unsupported type {model.GetType().Name}"),
        };

        /// <summary>
        /// The declared default of the property, as the value object its reference takes.
        /// </summary>
        public static object CreateDefaultValue(PropertyBaseXmlModel model) => model switch
        {
            PropertyBoolXmlModel m => m.DefaultValue,
            PropertyIntegerXmlModel m => (int) m.DefaultValue,
            PropertyFloatingIntegerXmlModel m => (float) m.DefaultValue,
            PropertyTextXmlModel m => m.DefaultValue,
            PropertyDropdownXmlModel m => new Dropdown<string>(m.Values, m.DefaultSelectedIndex),
            _ => throw new NotSupportedException($"Settings property '{model.Id}' has an unsupported type {model.GetType().Name}"),
        };

        /// <summary>
        /// The text a <c>Value</c> or <c>Default</c> attribute takes for the property's current value.
        /// </summary>
        public static string FormatValue(ISettingsPropertyDefinition property) => property.PropertyReference.Value switch
        {
            bool b => b ? "true" : "false",
            int i => i.ToString(CultureInfo.InvariantCulture),
            float f => f.ToString("R", CultureInfo.InvariantCulture),
            string s => s,
            null => string.Empty,
            var other when SettingsUtils.IsForGenericDropdown(other.GetType()) => new SelectedIndexWrapper(other).SelectedIndex.ToString(CultureInfo.InvariantCulture),
            var other => Convert.ToString(other, CultureInfo.InvariantCulture) ?? string.Empty,
        };

        /// <summary>
        /// Parses a value written the way a <c>Default</c> attribute is written. A dropdown takes the option index.
        /// </summary>
        public static bool TryParseValue(ISettingsPropertyDefinition property, string raw, out object? value)
        {
            switch (property.SettingType)
            {
                case SettingType.Bool when bool.TryParse(raw, out var b):
                    value = b;
                    return true;
                case SettingType.Int when int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i):
                    value = i;
                    return true;
                case SettingType.Float when float.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out var f):
                    value = f;
                    return true;
                case SettingType.String:
                    value = raw;
                    return true;
                case SettingType.Dropdown when property.PropertyReference.Value is Dropdown<string> dropdown
                                               && int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var index)
                                               && index >= 0 && index < dropdown.Count:
                    value = new Dropdown<string>(dropdown, index);
                    return true;
                default:
                    value = null;
                    return false;
            }
        }

        private static object? CloneValue(object? value) => value is ICloneable cloneable ? cloneable.Clone() : value;

        private ExternalGlobalSettings(string id, string displayName, int uiVersion, char subGroupDelimiter, PropertyChangedEventHandler? onPropertyChanged, IEnumerable<SettingsPropertyGroupDefinition> settingPropertyGroups, IEnumerable<ISettingsPresetBuilder> presets, string filePath)
            : base(id, displayName, string.Empty, string.Empty, DeclarationFormatType, uiVersion, subGroupDelimiter, onPropertyChanged, settingPropertyGroups, presets)
        {
            FilePath = filePath;
        }
    }
}
