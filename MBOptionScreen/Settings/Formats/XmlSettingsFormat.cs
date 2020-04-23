/*
using MBOptionScreen.Attributes;
using MBOptionScreen.Utils;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace MBOptionScreen.Settings
{
    [Version("e1.0.0",  200)]
    [Version("e1.0.1",  200)]
    [Version("e1.0.2",  200)]
    [Version("e1.0.3",  200)]
    [Version("e1.0.4",  200)]
    [Version("e1.0.5",  200)]
    [Version("e1.0.6",  200)]
    [Version("e1.0.7",  200)]
    [Version("e1.0.8",  200)]
    [Version("e1.0.9",  200)]
    [Version("e1.0.10", 200)]
    [Version("e1.0.11", 200)]
    [Version("e1.1.0",  200)]
    [Version("e1.2.0",  200)]
    internal sealed class XmlSettingsFormat : ISettingsFormat
    {
        public IEnumerable<string> Providers => new string[] { "xml" };

        public bool Save(SettingsBase settings, string path)
        {
            var formatter = settings is SettingsWrapper wrapperSettings ? CreateSerializer(wrapperSettings._object.GetType()) : CreateSerializer(settings.GetType());
            var file = new FileInfo(path);
            file.Directory?.Create();
            using var fs = file.Create();
            formatter.Serialize(fs, settings);
            return true;
        }

        // TODO: Populate like JSON is doing it
        public SettingsBase? Load(SettingsBase settings, string path)
        {
            var formatter = settings is SettingsWrapper wrapperSettings ? CreateSerializer(wrapperSettings._object.GetType()) : CreateSerializer(settings.GetType());
            var file = new FileInfo(path);
            file.Directory?.Create();
            using var fs = file.Create();
            return settings is SettingsWrapper ? new SettingsWrapper(formatter.Deserialize(fs)) : formatter.Deserialize(fs) as SettingsBase;
        }

        private static XmlSerializer CreateSerializer(Type serializableType)
        {
            var overrides = new XmlAttributeOverrides();

            for (var declaringType = serializableType; declaringType != null && declaringType != typeof(object); declaringType = declaringType.BaseType)
            {
                // check whether each property has the custom attribute
                foreach (var property in declaringType.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
                {
                    var attrbs = new XmlAttributes();

                    if (property.GetCustomAttributes().Any(a => ReflectionUtils.ImplementsOrImplementsEquivalent(a.GetType(), typeof(BaseSettingPropertyAttribute))))
                        attrbs.XmlElements.Add(new XmlElementAttribute(property.Name));
                    else
                        attrbs.XmlIgnore = true;
                    overrides.Add(declaringType, property.Name, attrbs);
                }
            }

            // create the serializer
            return new XmlSerializer(serializableType, overrides);
        }
    }
}
*/