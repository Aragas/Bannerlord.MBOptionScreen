using System;

namespace MCM.Abstractions.GameFeatures
{
#if !BANNERLORDMCM_PUBLIC
    internal
#else
    public
# endif
    interface IGameEventListener
    {
        public event Action OnGameStarted;
        public event Action OnGameEnded;
    }
}