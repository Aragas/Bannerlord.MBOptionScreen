using MCM.Utils;

using System.Linq;

namespace MCM.Abstractions.Settings
{
    public abstract class BaseGlobalSettingsWrapper : GlobalSettings, IWrapper
    {
        public static BaseGlobalSettingsWrapper Create(object @object) => DI.GetBaseInterfaceImplementations<BaseGlobalSettingsWrapper, GlobalSettingsWrapper>(args: @object)
            .FirstOrDefault(w => w.IsCorrect);

        public object Object { get; protected set; }

        public bool IsCorrect { get; protected set; }

        protected BaseGlobalSettingsWrapper(object @object)
        {
            Object = @object;
        }
    }
}