#if DEBUG
using MCM.Abstractions.Settings.Base.Global;

namespace MCM.Implementation.Testing
{
    public abstract class BaseTestGlobalSettings<T> : AttributeGlobalSettings<T> where T : GlobalSettings, new()
    {
        public override string FolderName => "Testing";

    }
}
#endif