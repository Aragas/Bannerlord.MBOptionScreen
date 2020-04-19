namespace MBOptionScreen.Settings.Wrapper
{
    internal abstract class SettingsWrapper : SettingsBase
    {
        internal readonly object _object;

        protected SettingsWrapper(object @object)
        {
            _object = @object;
        }
    }
}