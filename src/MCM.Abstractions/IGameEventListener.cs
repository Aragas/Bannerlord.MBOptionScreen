using System;

using TaleWorlds.Core;

namespace MCM.Abstractions
{
    public interface IGameEventListener
    {
        public event Action<Game> OnGameStarted;
        public event Action<Game> OnGameEnded;
    }
}