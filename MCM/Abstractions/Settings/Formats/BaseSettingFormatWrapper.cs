using HarmonyLib;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace MCM.Abstractions.Settings.Formats
{
    public abstract class BaseSettingFormatWrapper : ISettingsFormat, IWrapper
    {
        public object Object { get; }
        private PropertyInfo? ExtensionsProperty { get; }
        private MethodInfo? LoadMethod { get; }
        private MethodInfo? SaveMethod { get; }
        public virtual bool IsCorrect { get; }

        protected BaseSettingFormatWrapper(object @object)
        {
            Object = @object;
            var type = @object.GetType();

            ExtensionsProperty = AccessTools.Property(type, nameof(Extensions));
            LoadMethod = AccessTools.Method(type, nameof(Load));
            SaveMethod = AccessTools.Method(type, nameof(Save));

            IsCorrect = ExtensionsProperty != null && LoadMethod != null && SaveMethod != null;
        }

        public IEnumerable<string> Extensions => ExtensionsProperty?.GetValue(Object) as IEnumerable<string> ?? Array.Empty<string>();
        public BaseSettings? Load(BaseSettings settings, string path) => LoadMethod?.Invoke(Object, new object[] { settings, path }) as BaseSettings;
        public bool Save(BaseSettings settings, string path) => SaveMethod?.Invoke(Object, new object[] { settings, path }) as bool? ?? false;
    }
}