using MCM.Abstractions.Settings.Base.Global;

using System.Reflection;

namespace MCM.Abstractions.Settings.Containers.Global
{
    public abstract class FluentGlobalSettingsContainerWrapper : BaseSettingsContainerWrapper, IFluentGlobalSettingsContainer
    {
        private MethodInfo? RegisterMethod { get; }
        private MethodInfo? UnregisterMethod { get; }
        public override bool IsCorrect { get; }

        protected FluentGlobalSettingsContainerWrapper(object @object) : base(@object)
        {
            IsCorrect = base.IsCorrect && RegisterMethod != null && UnregisterMethod != null;
        }

        public void Register(FluentGlobalSettings settings) => RegisterMethod?.Invoke(Object, new object[] { settings });
        public void Unregister(FluentGlobalSettings settings) => UnregisterMethod?.Invoke(Object, new object[] { settings });
    }
}