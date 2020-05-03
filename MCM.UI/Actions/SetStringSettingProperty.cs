using MCM.Abstractions.Settings;

namespace MCM.UI.Actions
{
    internal sealed class SetStringSettingProperty : IAction
    {
        public IRef Context { get; }
        public object Value { get; }
        public object Original { get; }

        public SetStringSettingProperty(ISettingPropertyStringValue settingProperty, string value)
        {
            Context = new ProxyRef(() => settingProperty.StringValue, o => settingProperty.StringValue = o as string ?? "");
            Value = value;
            Original = settingProperty.StringValue;
        }

        public void DoAction() => Context.Value = (string) Value;
        public void UndoAction() => Context.Value = (string) Original;
    }
}