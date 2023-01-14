using Newtonsoft.Json;

using System.IO;
using System.Xml;

namespace MCM.Abstractions.Utils
{
    internal static class SerializationUtils
    {
        public static T? DeserializeXml<T>(Stream xmlStream)
        {
            // TODO: Not a correct workaround
            using var reader = new StreamReader(xmlStream);
            var xml = reader.ReadToEnd();
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return JsonConvert.SerializeXmlNode(doc) is { } json ? JsonConvert.DeserializeObject<T>(json) : default;
        }
    }
}