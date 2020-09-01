using MCM.Abstractions;
using MCM.Abstractions.Settings.Base;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace MCM.Implementation.Settings.Formats.Xml
{
    internal sealed class XmlSettingsFormat : BaseJsonSettingsFormat, IXmlSettingsFormat
    {
        public override IEnumerable<string> Extensions => new[] { "xml" };

        public XmlSettingsFormat(ILogger<XmlSettingsFormat> logger) : base(logger) { }

        public override bool Save(BaseSettings settings, string directoryPath, string filename)
        {
            var path = Path.Combine(directoryPath, filename + ".xml");

            var content = SaveJson(settings);
            var xmlDocument = JsonConvert.DeserializeXmlNode(content, settings is IWrapper wrapper1 ? wrapper1.Object.GetType().Name : settings.GetType().Name);

            var file = new FileInfo(path);
            file.Directory?.Create();
            var writer = file.CreateText();
            xmlDocument.Save(writer);
            writer.Dispose();

            return true;
        }

        public override BaseSettings? Load(BaseSettings settings, string directoryPath, string filename)
        {
            var path = Path.Combine(directoryPath, filename + ".xml");
            var file = new FileInfo(path);
            if (file.Exists)
            {
                var xmlDocument = new XmlDocument();
                var reader = file.OpenText();
                xmlDocument.Load(reader);
                reader.Dispose();

                var root = xmlDocument[settings.GetType().Name];
                if (root == null)
                {
                    Save(settings, directoryPath, filename);
                    return settings;
                }

                var content = JsonConvert.SerializeXmlNode(root, Newtonsoft.Json.Formatting.None, true);

                var set = LoadFromJson(settings, content);
                if (set == null)
                {
                    Save(settings, directoryPath, filename);
                    return settings;
                }
                else
                    return set;
            }
            else
            {
                Save(settings, directoryPath, filename);
                return settings;
            }
        }
    }
}