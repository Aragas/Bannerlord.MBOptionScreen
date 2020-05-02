using MBOptionScreen.Attributes;

using System.Collections.Generic;

namespace MBOptionScreen.Settings
{
    /*
    internal sealed class Test : AttributeSettings<Test>
    {
        public override string Id { get; set; } = "Test";
        public override string ModName => "Test";
        public override string ModuleFolderName => "";
        public override string Format => "firelord";
    }

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
    [Version("e1.2.1",  200)]
    [Version("e1.3.0",  200)]
    public class TT : ISettingsFormat
    {
        public IEnumerable<string> Extensions => new string[] { "firelord" };

        public SettingsBase? Load(SettingsBase settings, string path)
        {
            return settings;
        }

        public bool Save(SettingsBase settings, string path)
        {
            return true;
        }
    }
    */

    public interface ISettingsFormat
    {
        IEnumerable<string> Extensions { get; }

        bool Save(SettingsBase settings, string path);
        SettingsBase? Load(SettingsBase settings, string path);
    }
}