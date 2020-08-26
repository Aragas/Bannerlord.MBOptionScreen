using MCM.Abstractions.Settings.Models;

using System.Collections.Generic;
using System.ComponentModel;

namespace MCM.Abstractions.Settings.Base.Global
{
    public abstract class BaseFluentGlobalSettingsWrapper : FluentGlobalSettings, IWrapper
    {
        /// <inheritdoc/>
        public object Object { get; protected set; }

        protected BaseFluentGlobalSettingsWrapper(object @object, string id, string displayName, string folderName, string subFolder, string format,
            int uiVersion, char subGroupDelimiter, PropertyChangedEventHandler? onPropertyChanged, IEnumerable<SettingsPropertyGroupDefinition> settingPropertyGroups)
            : base(id, displayName, folderName, subFolder, format, uiVersion, subGroupDelimiter, onPropertyChanged, settingPropertyGroups)
        {
            Object = @object;
        }
    }
}