extern alias v5;

using NUnit.Framework;

using System;
using System.Runtime.CompilerServices;

using v5::MCM.Abstractions.Settings.Base.Global;
using v5::MCM.Abstractions.Settings.Formats;

namespace MCM.Tests.SettingsFormat
{
    public class BaseSettingsFormatTests : BaseTests
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        protected static bool MockedGetConfigPath(ref string __result)
        {
            __result = AppDomain.CurrentDomain.BaseDirectory;
            return false;
        }

        protected bool _boolValue;
        protected int _intValue;
        protected float _floatValue;
        protected string _stringValue = string.Empty;

        protected ISettingsFormat Format { get; set; } = default!;
        protected FluentGlobalSettings Settings { get; set; } = default!;
        protected string DirectoryPath { get; set; } = default!;
        protected string Filename { get; set; } = default!;

        [SetUp]
        public void Setup()
        {
            _boolValue = false;
            _intValue = 0;
            _floatValue = 0F;
            _stringValue = string.Empty;
        }
    }
}