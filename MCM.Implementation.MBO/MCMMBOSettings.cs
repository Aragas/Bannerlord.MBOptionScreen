using MCM.Abstractions.Settings.Base.Global;

namespace MCM.Implementation.MBO
{
    internal sealed class MCMMBOSettings : AttributeGlobalSettings<MCMMBOSettings>
    {
        public override string Id => "MCMMBO_v3";
        public override string DisplayName => $"MCM MBO Impl. {typeof(MCMMBOSettings).Assembly.GetName().Version.ToString(3)}";
        public override string FolderName => "MCM";
        public override string Format => "memory";
    }
}