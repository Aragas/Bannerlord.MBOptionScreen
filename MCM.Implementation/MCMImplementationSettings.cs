using MCM.Abstractions.Settings.Base.Global;

namespace MCM.Implementation
{
    internal sealed class MCMImplementationSettings : AttributeGlobalSettings<MCMImplementationSettings>
    {
        public override string Id => "MCMImplementation_v3";
        public override string DisplayName => $"MCM Impl. {typeof(MCMImplementationSettings).Assembly.GetName().Version.ToString(3)}";
        public override string FolderName => "MCM";
        public override string Format => "memory";
    }
}