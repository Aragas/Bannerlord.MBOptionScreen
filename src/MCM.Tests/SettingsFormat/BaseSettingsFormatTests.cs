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

        protected ISettingsFormat Format { get; set; } = default!;
        protected FluentGlobalSettings Settings { get; set; } = default!;
        protected string Path { get; set; } = default!;

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