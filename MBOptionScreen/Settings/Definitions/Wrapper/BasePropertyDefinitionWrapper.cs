namespace MBOptionScreen.Settings
{
    internal abstract class BasePropertyDefinitionWrapper : IPropertyDefinitionBase
    {
        protected readonly object _object;

        public string DisplayName { get; }
        public int Order { get; }
        public bool RequireRestart { get; }
        public string HintText { get; }

        protected BasePropertyDefinitionWrapper(object @object)
        {
            _object = @object;
            var type = @object.GetType();

            DisplayName = type.GetProperty("DisplayName")?.GetValue(@object) as string ?? "ERROR";
            Order = type.GetProperty("Order")?.GetValue(@object) as int? ?? -1;
            RequireRestart = type.GetProperty("RequireRestart")?.GetValue(@object) as bool? ?? true;
            HintText = type.GetProperty("HintText")?.GetValue(@object) as string ?? "ERROR";
        }
    }
}