using FridayNightFunkin;
using FridayNightFunkin.GamePlay;
using FridayNightFunkin.Settings;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace FridayNightFunkin.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] protected UnityEvent OnEscapePressed;
        protected FnfInput inputActions;

        private bool isInputedBy;

        private void Start()
        {
            Enabled(true);
        }

        public void Enabled(bool isEnabled)
        {
            if (isEnabled)
            {
                inputActions = InputManager.inputActions;
                inputActions.Enable();
            }
            else
            {
                inputActions = InputManager.inputActions;
                inputActions.Disable();
            }

        }
        protected virtual void Update()
        {
            if (!isPressed)
            {
                if ((inputActions.MenuNavigation.Escape.WasPerformedThisFrame() || inputActions.MenuNavigation.Pause.WasPressedThisFrame()) && !isInputedBy)
                {
                    Escape();
                    isInputedBy = true;
                }
                isInputedBy = false;
            }
            else
            {
                if (inputActions.MenuNavigation.Escape.WasPerformedThisFrame())
                {
                    Escape();
                    isInputedBy = false;
                }
            }
        }

        [SerializeField] protected UnityEvent OnEscapeUnpressed;
        private bool isPressed;
        public PlayableDirector director;

        public void Escape(bool callByButton = false)
        {
            isPressed = !isPressed;
            if (callByButton)
                isInputedBy = true;

            if (isPressed)
                OnEscapePressed?.Invoke();
            else
                OnEscapeUnpressed?.Invoke();

            GameStates();
        }

        private void GameStates()
        {
            GameState currentGameState = GameStateManager.instance.CurrentGameState;
            GameState newGameState = currentGameState == GameState.GamePlay ? GameState.Paused : GameState.GamePlay;

            GameStateManager.instance.SetState(newGameState);
        }

        public void RestartSong()
        {
            GameStateManager.instance.SetState(GameState.GamePlay);
            SceneLoad.instance.StartLoad(SceneManager.GetActiveScene().name);
        }

        public void ChangeDifficult(int difficult)
        {
            PlayerPrefs.SetInt("Difficult", difficult);
            RestartSong();
        }

        public void ExitToMenu()
        {
            GameStateManager.instance.SetState(GameState.GamePlay);
            LevelSettings.instance.SetStage(0);
            PlayerPrefs.SetInt("AfterLevel", 1);
            SceneLoad.instance.StartLoad("MainMenu");
        }
    }
}