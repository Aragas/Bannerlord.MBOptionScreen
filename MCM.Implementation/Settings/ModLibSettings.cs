using MCM.Abstractions.Settings;

namespace MCM.Implementation.Settings
{
    public sealed class ModLibSettings : SettingsWrapper
    {
        public ModLibSettings(object @object) : base(@object) { }

        internal void UpdateReference(object @object)
        {
            Object = @object;
        }
    }
}