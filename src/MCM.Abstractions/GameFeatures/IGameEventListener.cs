using System;

namespace MCM.Abstractions.GameFeatures
{
    public interface IGameEventListener
    {
        public event Action OnGameStarted;
        public event Action OnGameEnded;
    }
}