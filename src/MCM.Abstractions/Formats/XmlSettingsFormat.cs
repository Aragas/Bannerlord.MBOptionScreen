using BUTR.DependencyInjection;
using BUTR.DependencyInjection.Logger;

using MCM.Abstractions.Base;
using MCM.Abstractions.GameFeatures;
using MCM.Common;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Xml;

namespace MCM.Implementation
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    sealed class XmlSettingsFormat : BaseJsonSettingsFormat
    {
        public override IEnumerable<string> FormatTypes => new[] { "xml" };

        public XmlSettingsFormat(IBUTRLogger<XmlSettingsFormat> logger) : base(logger) { }

        public override bool Save(BaseSettings settings, GameDirectory directory, string filename)
        {
            if (GenericServiceProvider.GetService<IFileSystemProvider>() is not { } fileSystemProvider) return false;
            if (fileSystemProvider.GetOrCreateFile(directory, $"{filename}.xml") is not { } file) return false;

            var content = SaveJson(settings);
            var xmlDocument = JsonConvert.DeserializeXmlNode(content, settings is IWrapper { Object: { } obj } ? obj.GetType().Name : settings.GetType().Name);
            if (xmlDocument is null) return false;

            using var ms = new System.IO.MemoryStream();
            using var writer = new System.IO.StreamWriter(ms);
            xmlDocument.Save(writer);
            
            try
            {
                return fileSystemProvider.WriteData(file, ms.ToArray());
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override BaseSettings Load(BaseSettings settings, GameDirectory directory, string filename)
        {
            if (GenericServiceProvider.GetService<IFileSystemProvider>() is not { } fileSystemProvider) return settings;
            if (fileSystemProvider.GetFile(directory, $"{filename}.xml") is not { } file) return settings;
            if (fileSystemProvider.ReadData(file) is not { } data) 
            {
                Save(settings, directory, filename);
                return settings;
            }
            
            var xmlDocument = new XmlDocument();
            using var ms = new System.IO.MemoryStream(data);
            xmlDocument.Load(ms);

            var root = xmlDocument[settings.GetType().Name];
            if (root is null)
            {
                Save(settings, directory, filename);
                return settings;
            }

            var content = JsonConvert.SerializeXmlNode(root, Newtonsoft.Json.Formatting.None, true);

            if (!TryLoadFromJson(ref settings, content))
                Save(settings, directory, filename);
                
            return settings;

        }
    }
}