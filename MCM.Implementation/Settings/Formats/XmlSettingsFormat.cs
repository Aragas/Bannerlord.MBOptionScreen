using MCM.Abstractions.Attributes;
using MCM.Abstractions.Settings;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.IO;
using System.Xml;

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
    internal sealed class XmlSettingsFormat : JsonSettingsFormat
    {
        public override IEnumerable<string> Extensions => new string[] { "xml" };

        public override bool Save(SettingsBase settings, string path)
        {
            var content = settings is SettingsWrapper wrapperSettings
                ? JsonConvert.SerializeObject(wrapperSettings.Object, _jsonSerializerSettings)
                : JsonConvert.SerializeObject(settings, _jsonSerializerSettings);
            var xmlDocument = JsonConvert.DeserializeXmlNode(content, settings is SettingsWrapper wrapperSettings1 ? wrapperSettings1.Object.GetType().Name : settings.GetType().Name);

            var file = new FileInfo(path);
            file.Directory?.Create();
            using var writer = file.CreateText();
            xmlDocument.Save(writer);

            return true;
        }

        public override SettingsBase? Load(SettingsBase settings, string path)
        {
            var file = new FileInfo(path);
            if (file.Exists)
            {
                try
                {
                    using var reader = file.OpenText();
                    var xmlDocument = new XmlDocument();
                    xmlDocument.Load(reader);
                    var content = JsonConvert.SerializeXmlNode(xmlDocument);

                    if (settings is SettingsWrapper wrapperSettings)
                        JsonConvert.PopulateObject(content, wrapperSettings.Object, _jsonSerializerSettings);
                    else
                        JsonConvert.PopulateObject(content, settings, _jsonSerializerSettings);
                }
                catch (JsonException)
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
    }
}