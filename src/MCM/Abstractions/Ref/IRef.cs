using System;
using System.ComponentModel;

namespace MCM.Abstractions.Ref
{
    public interface IRef : INotifyPropertyChanged
    {
        Type Type { get; }
        object Value { get; set; }
    }
}