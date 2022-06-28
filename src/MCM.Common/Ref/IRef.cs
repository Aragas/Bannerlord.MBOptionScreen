using System;
using System.ComponentModel;

namespace MCM.Common
{
    /// <summary>
    /// An interface that provides access to some value for MCM to get/set.
    /// </summary>
    public interface IRef : INotifyPropertyChanged
    {
        /// <summary>
        /// Underlying type of the value
        /// </summary>
        Type Type { get; }
        object? Value { get; set; }
    }
}