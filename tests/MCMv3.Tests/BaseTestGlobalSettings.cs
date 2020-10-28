using MCM.Abstractions.Settings.Base.Global;

namespace MCMv3.Tests
{
    public abstract class BaseTestGlobalSettings<T> : AttributeGlobalSettings<T> where T : GlobalSettings, new()
    {
        public override string FolderName => "Testing";
    }
}