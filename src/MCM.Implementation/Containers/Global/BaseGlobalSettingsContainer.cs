using BUTR.DependencyInjection;

using MCM.Abstractions.Base.Global;
using MCM.Abstractions.GameFeatures;
using MCM.Abstractions.Global;

namespace MCM.Implementation.Global
{
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
    internal abstract class BaseGlobalSettingsContainer : BaseSettingsContainer<GlobalSettings>, IGlobalSettingsContainer
    {
        /// <inheritdoc/>
        protected override GameDirectory RootFolder { get; }

        protected BaseGlobalSettingsContainer()
        {
            var fileSystemProvider = GenericServiceProvider.GetService<IFileSystemProvider>();
            RootFolder = fileSystemProvider?.GetDirectory(base.RootFolder, "Global") ?? base.RootFolder;
        }
    }
}