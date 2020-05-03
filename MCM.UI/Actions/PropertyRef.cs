using System.Reflection;

namespace MCM.UI.Actions
{
    internal class PropertyRef : IRef
    {
        public PropertyInfo PropertyInfo { get; }
        public object Instance { get; }

        public object Value { get => PropertyInfo.GetValue(Instance); set => PropertyInfo.SetValue(Instance, value); }

        public PropertyRef(PropertyInfo propInfo, object instance)
        {
            PropertyInfo = propInfo;
            Instance = instance;
        }
    }
}