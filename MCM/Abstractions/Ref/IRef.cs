using System;
using System.ComponentModel;

namespace MCM.Abstractions.Ref
{
    /// <summary>
    /// An interface that provides access to some value for MCM to get/set.
    /// </summary>
    public interface IRef : INotifyPropertyChanged
    {
        Type Type { get; }
        object Value { get; set; }
    }
}
