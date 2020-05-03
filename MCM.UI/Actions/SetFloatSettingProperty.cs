using MCM.Abstractions.Settings;

namespace MCM.UI.Actions
{
    internal sealed class SetFloatSettingProperty : IAction
    {
        public IRef Context { get; }
        public object Value { get; }
        public object Original { get; }

        public SetFloatSettingProperty(ISettingPropertyFloatValue settingProperty, float value)
        {
            Context = new ProxyRef(() => settingProperty.FloatValue, o => settingProperty.FloatValue = o as int? ?? 0);
            Value = value;
            Original = settingProperty.FloatValue;
        }

        public void DoAction() => Context.Value = (float) Value;
        public void UndoAction() => Context.Value = (float) Original;
    }
}