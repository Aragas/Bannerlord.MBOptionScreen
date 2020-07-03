// Copyright 2019 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using Microsoft.DocAsCode.Build.Engine;
using Microsoft.DocAsCode.Plugins;
using Mono.Cecil;
using SharpCompress.Archives.Zip;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace SandcastleXrefGenerator
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            if (args.Length != 5)
            {
                Console.WriteLine("Usage: <nuget package> <version> <tfm> <baseUrl> <outputFile>");
                return 1;
            }

            var packageStream = await DownloadPackage(args[0], args[1]);
            var module = LoadModule(packageStream, args[2]);

            var map = BuildXRefMap(module, args[3]);
            var serializer = new SerializerBuilder().Build();
            var file = new FileInfo(args[4]);
            file.Directory!.Create();
            serializer.Serialize(file.CreateText(), map);
            return 0;
        }

        private static async Task<Stream> DownloadPackage(string package, string version)
        {
            var client = new HttpClient();
            var bytes = await client.GetByteArrayAsync($"https://www.nuget.org/api/v2/package/{package}/{version}");
            return new MemoryStream(bytes);
        }

        private static ModuleDefinition LoadModule(Stream package, string tfm)
        {
            var prefix = $"lib/{tfm}";
            using var zip = ZipArchive.Open(package);
            foreach (var entry in zip.Entries)
            {
                // We assume there's just one assembly, for simplicity.
                if (entry.Key.StartsWith(prefix) && entry.Key.EndsWith(".dll"))
                {
                    using var stream = entry.OpenEntryStream();
                    // Mono.Cecil requires the stream to be seekable. It's simplest
                    // just to copy the whole DLL to a MemoryStream and pass that to Cecil.
                    var ms = new MemoryStream();
                    stream.CopyTo(ms);
                    ms.Position = 0;
                    return ModuleDefinition.ReadModule(ms);
                }
            }

            throw new Exception($"No file found in package starting with '{prefix}'");
        }

        private static XRefMap BuildXRefMap(ModuleDefinition module, string baseUrl) => new XRefMap
        {
            BaseUrl = baseUrl,
            References = module.Types
                .Select(CreateXRefSpec)
                .Where(spec => spec != null)
                .ToList()
        };

        private static XRefSpec CreateXRefSpec(TypeDefinition type)
        {
            if (!type.IsPublic)
            {
                return null;
            }
            return new XRefSpec
            {
                Name = type.Name, // TODO: Get the name with <T> etc in
                Uid = type.FullName, // Is this really enough?
                Href = $"T_{type.FullName.Replace('.', '_').Replace('`', '_')}.htm",
                CommentId = $"T:{type.FullName}"
            };
        }
    }
}