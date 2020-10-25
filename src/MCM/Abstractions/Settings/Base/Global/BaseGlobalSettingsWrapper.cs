namespace MCM.Abstractions.Settings.Base.Global
{
    public abstract class BaseGlobalSettingsWrapper : GlobalSettings, IWrapper
    {
        /// <inheritdoc/>
        public object Object { get; protected set; }

        protected BaseGlobalSettingsWrapper(object @object)
        {
            Object = @object;
        }
    }
}