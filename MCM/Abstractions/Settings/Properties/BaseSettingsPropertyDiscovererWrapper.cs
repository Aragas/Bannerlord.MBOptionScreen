using HarmonyLib;

using MCM.Abstractions.Settings.Models;
using MCM.Abstractions.Settings.Models.Wrapper;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MCM.Abstractions.Settings.Properties
{
    public abstract class BaseSettingsPropertyDiscovererWrapper : ISettingsPropertyDiscoverer, IWrapper
    {
        public object Object { get; }
        private MethodInfo? GetPropertiesMethod { get; }
        public virtual bool IsCorrect { get; }

        protected BaseSettingsPropertyDiscovererWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();

            GetPropertiesMethod = AccessTools.Method(type, nameof(GetProperties));

            IsCorrect = GetPropertiesMethod != null;
        }

        public IEnumerable<ISettingsPropertyDefinition> GetProperties(object @object) =>
            ((IEnumerable<object>) (GetPropertiesMethod?.Invoke(Object, new object[] { @object }) ?? new List<object>()))
            .Select(o => new SettingsPropertyDefinitionWrapper(o));
    }
}