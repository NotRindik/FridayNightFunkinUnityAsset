using FnF.Scripts;
using FnF.Scripts.Extensions;
using FridayNightFunkin.Editor.TimeLineEditor;
using FridayNightFunkin.GamePlay;
using UnityEngine;
using UnityEngine.Events;
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
                inputActions = InputManager.InputActions;
                inputActions.Enable();
            }
            else
            {
                inputActions = InputManager.InputActions;
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
            G.Instance.Get<SceneLoad>().StartLoad(SceneManager.GetActiveScene().name);
        }

        public void ChangeDifficult(int difficult)
        {
            PlayerPrefs.SetInt("Difficult", difficult);
            RestartSong();
        }

        public void ExitToMenu()
        {
            GameStateManager.instance.SetState(GameState.GamePlay);
            PlayerPrefs.SetInt(LevelManager.STAGE_PLAYERPREFS_NAME, 0);
            PlayerPrefs.SetInt("AfterLevel", 1);
            G.Instance.Get<SceneLoad>().StartLoad("MainMenu");
        }
    }
}