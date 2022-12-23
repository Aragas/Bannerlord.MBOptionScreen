using MCM.Abstractions.Base.Global;
using MCM.Abstractions.Global;

using System.IO;

namespace MCM.Implementation.Global
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
    internal abstract class BaseGlobalSettingsContainer : BaseSettingsContainer<GlobalSettings>, IGlobalSettingsContainer
    {
        /// <inheritdoc/>
        protected override string RootFolder { get; }

        protected BaseGlobalSettingsContainer()
        {
            RootFolder = Path.Combine(base.RootFolder, "Global");
        }
    }
}