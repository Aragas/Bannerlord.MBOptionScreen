using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MBOptionScreen
{
    // ModLib Stub - second attempt
    //
    // Still won't work. Because ModLib settings inherit from ModLib.dll,
    // the game will attempt to load ModLib.dll at that stage.
    // The only workaround for it would be the ability to use DllMain
    // And it will require the ability to call managed code
    // and access the managed context.
    //
    // Module Initializers won't work, called at the same stage as
    // type static constrcutors
    internal static class ModLibStub
    {
        private static byte[] ReadFully(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using MemoryStream ms = new MemoryStream();
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                ms.Write(buffer, 0, read);
            return ms.ToArray();
        }

        public static void LoadIfNeeded()
        {
            var modLibLoaded = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .Any(a => Path.GetFileNameWithoutExtension(a.Location) == "ModLib");
            if (!modLibLoaded)
            {
                /*
                using var modLibStubAssemblyStream = typeof(ModLibStub).Assembly.GetManifestResourceStream("MBOptionScreen._Data.ModLib.dll");
                Assembly.Load(ReadFully(modLibStubAssemblyStream));
                */
                /*
                var thisAssemblyFile = new FileInfo(typeof(ModLibStub).Assembly.Location);
                var modLibStubPath = thisAssemblyFile.Directory.GetFiles("ModLib.dll").FirstOrDefault();
                if (modLibStubPath != null)
                    AssemblyLoader.LoadFrom(modLibStubPath.FullName);
                */
            }
        }
    }
}