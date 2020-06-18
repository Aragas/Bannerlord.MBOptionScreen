using System;

namespace MCM.Abstractions.Settings.Definitions
{
    public interface IPropertyGroupDefinition
    {
        string GroupName { get; }
        [Obsolete("Will be removed", true)]
        bool IsMainToggle { get; }
        int GroupOrder { get; }
    }
}