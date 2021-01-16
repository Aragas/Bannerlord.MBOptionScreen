namespace MCM.Abstractions.Settings.Base.PerCharacter
{
    public abstract class BasePerCharacterSettingsWrapper : PerCharacterSettings, IDependencyBase, IWrapper
    {
        public static BasePerCharacterSettingsWrapper Create(object @object) => null!;

        public object Object { get; set; }
        public bool IsCorrect { get; set; }

        protected BasePerCharacterSettingsWrapper(object @object) { }
    }
}