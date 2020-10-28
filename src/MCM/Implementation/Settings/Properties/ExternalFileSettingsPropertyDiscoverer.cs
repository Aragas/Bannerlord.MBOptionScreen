using MCM.Abstractions;
using MCM.Abstractions.Settings.Base;
using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Models;
using MCM.Abstractions.Settings.Properties;

using System.Collections.Generic;
using System.IO;
using System.Xml;

using TaleWorlds.Engine;

using Path = System.IO.Path;

namespace MCM.Implementation.Settings.Properties
{
    internal sealed class ExternalFileSettingsPropertyDiscoverer : IAttributeSettingsPropertyDiscoverer
    {
        public IEnumerable<string> DiscoveryTypes { get; } = new [] { "external-file-xml" };

        public IEnumerable<ISettingsPropertyDefinition> GetProperties(BaseSettings settings)
        {
            var externalGlobalSettings = settings switch
            {
                IWrapper wrapper => wrapper.Object as ExternalGlobalSettings,
                _ => settings as ExternalGlobalSettings
            };

            if (externalGlobalSettings == null)
                yield break;

        }

        private static IEnumerable<ISettingsPropertyDefinition> GetPropertiesInternal(ExternalGlobalSettings settings)
        {
            var path = Path.Combine(Utilities.GetBasePath(), "Modules", settings.FolderName, settings.SubFolder, $"{settings.Id}.xml");
            if (!File.Exists(path))
                yield break;

            var doc = new XmlDocument();
            using XmlReader reader = XmlReader.Create(path, new XmlReaderSettings { IgnoreComments = true, IgnoreWhitespace = true });
            doc.Load(reader);
        }
    }

    internal sealed class ExternalEmbedSettingsPropertyDiscoverer : IAttributeSettingsPropertyDiscoverer
    {
        public IEnumerable<string> DiscoveryTypes { get; } = new [] { "external-embed-xml" };

        public IEnumerable<ISettingsPropertyDefinition> GetProperties(BaseSettings settings)
        {
            var externalGlobalSettings = settings switch
            {
                IWrapper wrapper => wrapper.Object as ExternalGlobalSettings,
                _ => settings as ExternalGlobalSettings
            };

            if (externalGlobalSettings == null)
                yield break;

        }

        private static IEnumerable<ISettingsPropertyDefinition> GetPropertiesInternal(ExternalGlobalSettings settings)
        {
            var path = Path.Combine(Utilities.GetBasePath(), "Modules", settings.FolderName, settings.SubFolder, $"{settings.Id}.xml");
            var stream = settings.GetType().Assembly.GetManifestResourceStream(path);
            if (stream == null)
                yield break;

            var doc = new XmlDocument();
            using XmlReader reader = XmlReader.Create(stream, new XmlReaderSettings { IgnoreComments = true, IgnoreWhitespace = true });
            doc.Load(reader);
        }
    }
}