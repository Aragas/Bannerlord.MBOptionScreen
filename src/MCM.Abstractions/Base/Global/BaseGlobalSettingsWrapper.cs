using MCM.Common;

using System;

namespace MCM.Abstractions.Base.Global
{
    [Obsolete("Will be removed from future API", true)]
#if !BANNERLORDMCM_INCLUDE_IN_CODE_COVERAGE
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage, global::System.Diagnostics.DebuggerNonUserCode]
#endif
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    abstract class BaseGlobalSettingsWrapper : GlobalSettings, IWrapper
    {
        /// <inheritdoc/>
        public object Object { get; protected set; }

        protected BaseGlobalSettingsWrapper(object @object)
        {
            Object = @object;
        }
    }
}