using MBOptionScreen.Interfaces;

using System;

namespace MBOptionScreen.State
{
    internal class SharedStateObjectV1 : ISharedStateObject
    {
        public Type ModOptionScreen { get; internal set; }

        public SharedStateObjectV1(Type modOptionScreen)
        {
            ModOptionScreen = modOptionScreen;
        }
    }
}