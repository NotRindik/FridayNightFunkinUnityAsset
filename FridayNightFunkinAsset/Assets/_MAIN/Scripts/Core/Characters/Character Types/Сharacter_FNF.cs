using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FridayNightFunkin.CHARACTERS
{
    public abstract class Сharacter_FNF : MonoBehaviour
    {
        protected Animator animator;
        protected string currentAnimationState;

        protected string[] SING_NOTES = { "Left", "Down", "Up", "Right" };
        protected const string IDLE = "Idle";
        protected const string ARROW_PRESSED = "Pressed";

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            GameStateManager.instance.OnGameStateChanged += OnGameStateChanged;
        }
        public void PlayNote(ArrowSide arrowSide)
        {
            animator.CrossFade(SING_NOTES[(int)arrowSide],0f);
        }
        public void PlayNote(int arrowSide)
        {
            animator.CrossFade(SING_NOTES[arrowSide], 0f);
        }
        public string ReturnNote(int arrowSide)
        {
            return SING_NOTES[arrowSide];
        }

        private void OnGameStateChanged(GameState newGameState)
        {
            animator.speed = newGameState == GameState.Paused? 0 : 1;
        }

        private void OnDestroy()
        {
            GameStateManager.instance.OnGameStateChanged -= OnGameStateChanged;
        }
    }
}