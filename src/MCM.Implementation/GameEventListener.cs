using MCM.Abstractions;

using System;

using TaleWorlds.Core;

namespace MCM.Implementation
{
    internal sealed class GameEventListener : IGameEventListener
    {
        /// <inheritdoc />
        public event Action<Game>? OnGameStarted;

        /// <inheritdoc />
        public event Action<Game>? OnGameEnded;

        public void GameStarted(Game game)
        {
            OnGameStarted?.Invoke(game);
        }
        
        public void GameEnded(Game game)
        {
            OnGameEnded?.Invoke(game);
        }
    }
}