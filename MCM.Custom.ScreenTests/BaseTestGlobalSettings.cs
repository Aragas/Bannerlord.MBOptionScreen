using MCM.Abstractions.Settings.Base.Global;

namespace MCM.Custom.ScreenTests
{
    public abstract class BaseTestGlobalSettings<T> : AttributeGlobalSettings<T> where T : GlobalSettings, new()
    {
        public override string FolderName => "Testing";

    }
}