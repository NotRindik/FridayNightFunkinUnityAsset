using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    GamePlay,
    Paused
}
namespace FridayNightFunkin.GamePlay
{
    public class GameStateManager
    {
        private static GameStateManager _instance;

        public static GameStateManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameStateManager();
                }

                return _instance;
            }
        }

        public GameState CurrentGameState { get; private set; }

        public delegate void GameStateChangeHandler(GameState gameState);
        public event GameStateChangeHandler OnGameStateChanged;

        private GameStateManager()
        {

        }

        public void SetState(GameState currenState)
        {
            if (currenState == CurrentGameState)
                return;

            CurrentGameState = currenState;
            OnGameStateChanged?.Invoke(currenState);
        }
    }
}