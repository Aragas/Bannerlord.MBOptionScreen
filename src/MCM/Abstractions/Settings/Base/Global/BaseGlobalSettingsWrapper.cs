namespace MCM.Abstractions.Settings.Base.Global
{
    public abstract class BaseGlobalSettingsWrapper : GlobalSettings, IDependencyBase, IWrapper
    {
        public static BaseGlobalSettingsWrapper Create(object @object) => null!;

        public object Object { get; protected set; }
        public bool IsCorrect { get; protected set; }

        protected BaseGlobalSettingsWrapper(object @object) { }
    }
}