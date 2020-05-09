using HarmonyLib;

using MCM.Abstractions.Settings.Models;
using MCM.Abstractions.Settings.Models.Wrapper;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MCM.Abstractions.Settings.Properties
{
    public sealed class SettingsPropertyDiscovererWrapper : ISettingsPropertyDiscoverer, IWrapper
    {
        public object Object { get; }
        private MethodInfo? GetPropertiesMethod { get; }
        public bool IsCorrect { get; }

        public SettingsPropertyDiscovererWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();

            GetPropertiesMethod = AccessTools.Method(type, nameof(GetProperties));

            IsCorrect = GetPropertiesMethod != null;
        }

        public IEnumerable<ISettingsPropertyDefinition> GetProperties(object @object, string id) =>
            ((IEnumerable<object>) (GetPropertiesMethod?.Invoke(Object, new object[] { @object, id }) ?? new List<object>()))
            .Select(o => new SettingsPropertyDefinitionWrapper(o));
    }
}