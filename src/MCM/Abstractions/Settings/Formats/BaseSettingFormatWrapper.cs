using MCM.Abstractions.Settings.Base;

using System.Collections.Generic;
using System.Linq;

namespace MCM.Abstractions.Settings.Formats
{
    public abstract class BaseSettingFormatWrapper : ISettingsFormat, IWrapper
    {
        public object Object { get; }
        public virtual bool IsCorrect { get; }

        protected BaseSettingFormatWrapper(object @object) { }

        public IEnumerable<string> Extensions => Enumerable.Empty<string>();
        public BaseSettings? Load(BaseSettings settings, string path) => null;
        public bool Save(BaseSettings settings, string path) => false;
    }
}