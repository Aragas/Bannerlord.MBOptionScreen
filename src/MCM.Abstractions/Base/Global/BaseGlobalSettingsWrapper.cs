using MCM.Common;

using System;

namespace MCM.Abstractions.Base.Global
{
    [Obsolete("Will be removed from future API", true)]
    public abstract class BaseGlobalSettingsWrapper : GlobalSettings, IWrapper
    {
        /// <inheritdoc/>
        public object Object { get; protected set; }

        protected BaseGlobalSettingsWrapper(object @object)
        {
            Object = @object;
        }
    }
}