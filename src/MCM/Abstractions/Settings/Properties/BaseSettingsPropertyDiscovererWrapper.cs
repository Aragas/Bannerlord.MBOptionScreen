using MCM.Abstractions.Settings.Models;

using System.Collections.Generic;
using System.Linq;

namespace MCM.Abstractions.Settings.Properties
{
    public abstract class BaseSettingsPropertyDiscovererWrapper : ISettingsPropertyDiscoverer, IWrapper
    {
        public static BaseSettingsPropertyDiscovererWrapper Create(object @object) => null!;


        public object Object { get; }
        public virtual bool IsCorrect { get; }

        protected BaseSettingsPropertyDiscovererWrapper(object @object) { }

        public IEnumerable<ISettingsPropertyDefinition> GetProperties(object @object) => Enumerable.Empty<ISettingsPropertyDefinition>();
    }
}