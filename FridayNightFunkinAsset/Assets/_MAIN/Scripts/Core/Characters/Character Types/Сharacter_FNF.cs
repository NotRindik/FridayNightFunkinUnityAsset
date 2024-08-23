using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FridayNightFunkin.CHARACTERS
{
    public class Сharacter_FNF : MonoBehaviour
    {
        public Animator animator;
        protected string currentAnimationState;

        private string[] SING_NOTES = { "Left", "Down", "Up", "Right" };
        protected const string IDLE = "Idle";
        protected const string ARROW_PRESSED = "Pressed";

        private void Awake()
        {
            animator = GetComponent<Animator>();
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
    }
}