using MCM.UI.Exceptions;

using System.Xml;

namespace MCM.UI.Functionality.Injectors
{
    public abstract class ResourceInjector
    {
        protected static XmlDocument Load(string embedPath)
        {
            using var stream = typeof(DefaultResourceInjector).Assembly.GetManifestResourceStream(embedPath);
            if (stream is null) throw new MCMUIEmbedResourceNotFoundException($"Could not find embed resource '{embedPath}'!");
            using var xmlReader = XmlReader.Create(stream, new XmlReaderSettings { IgnoreComments = true });
            var doc = new XmlDocument();
            doc.Load(xmlReader);
            return doc;
        }

        public abstract void Inject();
    }
}