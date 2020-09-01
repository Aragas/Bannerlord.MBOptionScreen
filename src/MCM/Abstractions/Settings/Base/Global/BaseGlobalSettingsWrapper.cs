using MCM.Abstractions.Settings.Models;
using MCM.Utils;

using System.Collections.Generic;
using System.Linq;

namespace MCM.Abstractions.Settings.Base.Global
{
    public abstract class BaseGlobalSettingsWrapper : GlobalSettings, IWrapper
    {
        /// <inheritdoc/>
        public object Object { get; protected set; }

        protected BaseGlobalSettingsWrapper(object @object)
        {
            Object = @object;
        }

        /// <inheritdoc/>
        protected override IEnumerable<SettingsPropertyGroupDefinition> GetUnsortedSettingPropertyGroups() =>
            SettingsUtils.GetSettingsPropertyGroups(SubGroupDelimiter, Discoverer?.GetProperties(Object) ?? Enumerable.Empty<ISettingsPropertyDefinition>());
    }
}