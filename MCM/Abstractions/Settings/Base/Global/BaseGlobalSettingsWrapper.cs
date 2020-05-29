using MCM.Abstractions.Settings.Models;
using MCM.Utils;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MCM.Abstractions.Settings.Base.Global
{
    public abstract class BaseGlobalSettingsWrapper : GlobalSettings, IDependencyBase, IWrapper
    {
        public static BaseGlobalSettingsWrapper Create(object @object) => DI.GetBaseImplementations<BaseGlobalSettingsWrapper, GlobalSettingsWrapper>(@object)
            .FirstOrDefault(w => w.IsCorrect);

        public object Object { get; protected set; }
        public bool IsCorrect { get; protected set; }

        protected BaseGlobalSettingsWrapper(object @object)
        {
            Object = @object;
        }

        protected override BaseSettings CreateNew()
        {
            var newObject = Activator.CreateInstance(Object.GetType());
            return Create(newObject);
        }

        protected override IEnumerable<SettingsPropertyGroupDefinition> GetUnsortedSettingPropertyGroups() =>
            SettingsUtils.GetSettingsPropertyGroups(SubGroupDelimiter, Discoverer?.GetProperties(Object) ?? Array.Empty<ISettingsPropertyDefinition>());
    }
}