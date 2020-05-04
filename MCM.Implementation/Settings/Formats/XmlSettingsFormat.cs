using MCM.Abstractions.Attributes;
using MCM.Abstractions.Settings;
using MCM.Abstractions.Settings.Definitions;
using MCM.Abstractions.Settings.Formats;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace MCM.Implementation.Settings.Formats
{
    [Version("e1.0.0",  1)]
    [Version("e1.0.1",  1)]
    [Version("e1.0.2",  1)]
    [Version("e1.0.3",  1)]
    [Version("e1.0.4",  1)]
    [Version("e1.0.5",  1)]
    [Version("e1.0.6",  1)]
    [Version("e1.0.7",  1)]
    [Version("e1.0.8",  1)]
    [Version("e1.0.9",  1)]
    [Version("e1.0.10", 1)]
    [Version("e1.0.11", 1)]
    [Version("e1.1.0",  1)]
    [Version("e1.2.0",  1)]
    [Version("e1.2.1",  1)]
    [Version("e1.3.0",  1)]
    internal sealed class XmlSettingsFormat : ISettingsFormat
    {
        public IEnumerable<string> Extensions => new string[] { "xml" };

        public bool Save(SettingsBase settings, string path)
        {
            var formatter = settings is SettingsWrapper wrapperSettings ? CreateSerializer(wrapperSettings.Object.GetType()) : CreateSerializer(settings.GetType());
            
            var file = new FileInfo(path);
            file.Directory?.Create();
            using var fs = file.Create();
            formatter.Serialize(fs, settings is SettingsWrapper settingsWrapper ? settingsWrapper.Object : settings);

            return true;
        }

        public SettingsBase? Load(SettingsBase settings, string path)
        {
            var file = new FileInfo(path);
            if (file.Exists)
            {
                var formatter = settings is SettingsWrapper wrapperSettings ? CreateSerializer(wrapperSettings.Object.GetType()) : CreateSerializer(settings.GetType());

                try
                {
                    using var fs = file.OpenRead();
                    if (settings is SettingsWrapper)
                        SettingsUtils.OverrideSettings(settings, new SettingsWrapper(formatter.Deserialize(fs)));
                    else
                        SettingsUtils.OverrideSettings(settings, (SettingsBase) formatter.Deserialize(fs));
                }
                catch (Exception)
                {
                    Save(settings, path);
                }
            }
            else
            {
                Save(settings, path);
            }
            return settings;
        }

        private static XmlSerializer CreateSerializer(Type serializableType)
        {
            var overrides = new XmlAttributeOverrides();

            for (var declaringType = serializableType; declaringType != null && declaringType != typeof(object); declaringType = declaringType.BaseType)
            {
                // check whether each property has the custom attribute
                foreach (var property in declaringType.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
                {
                    var attr = new XmlAttributes();

                    if (property.GetCustomAttributes().Any(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), "MBOptionScreen.Settings.IPropertyGroupDefinition") ||
                                                                ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(IPropertyGroupDefinition))))
                        attr.XmlElements.Add(new XmlElementAttribute(property.Name));
                    else
                        attr.XmlIgnore = true;
                    overrides.Add(declaringType, property.Name, attr);
                }
            }

            // create the serializer
            return new XmlSerializer(serializableType, overrides);
        }
    }
}