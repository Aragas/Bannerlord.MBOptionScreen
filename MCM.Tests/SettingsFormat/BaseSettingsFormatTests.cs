using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Formats;

using NUnit.Framework;

namespace MCM.Tests.SettingsFormat
{
    public class BaseSettingsFormatTests
    {
        protected bool _boolValue;
        protected int _intValue;
        protected float _floatValue;
        protected string _stringValue = "";

        protected ISettingsFormat Format { get; set; }
        protected FluentGlobalSettings Settings { get; set; }
        protected string Path { get; set;}

        [SetUp]
        public void Setup()
        {
            _boolValue = false;
            _intValue = 0;
            _floatValue = 0F;
            _stringValue = "";
        }
    }
}