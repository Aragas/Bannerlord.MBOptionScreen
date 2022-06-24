﻿using BUTR.DependencyInjection.Logger;

using MCM.Abstractions;
using MCM.Abstractions.Settings.Base;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace MCM.Implementation.Settings.Formats
{
    public sealed class XmlSettingsFormat : BaseJsonSettingsFormat
    {
        public override IEnumerable<string> FormatTypes => new[] { "xml" };

        public XmlSettingsFormat(IBUTRLogger<XmlSettingsFormat> logger) : base(logger) { }

        public override bool Save(BaseSettings settings, string directoryPath, string filename)
        {
            var path = Path.Combine(directoryPath, $"{filename}.xml");

            var content = SaveJson(settings);
            var xmlDocument = JsonConvert.DeserializeXmlNode(content, settings is IWrapper wrapper1 ? wrapper1.Object.GetType().Name : settings.GetType().Name);

            var file = new FileInfo(path);
            file.Directory?.Create();
            var writer = file.CreateText();
            xmlDocument.Save(writer);
            writer.Dispose();

            return true;
        }

        public override BaseSettings Load(BaseSettings settings, string directoryPath, string filename)
        {
            var path = Path.Combine(directoryPath, $"{filename}.xml");
            var file = new FileInfo(path);
            if (file.Exists)
            {
                var xmlDocument = new XmlDocument();
                var reader = file.OpenText();
                xmlDocument.Load(reader);
                reader.Dispose();

                var root = xmlDocument[settings.GetType().Name];
                if (root is null)
                {
                    Save(settings, directoryPath, filename);
                    return settings;
                }

                var content = JsonConvert.SerializeXmlNode(root, Newtonsoft.Json.Formatting.None, true);

                return LoadFromJson(settings, content);
            }
            else
            {
                Save(settings, directoryPath, filename);
                return settings;
            }
        }
    }
}