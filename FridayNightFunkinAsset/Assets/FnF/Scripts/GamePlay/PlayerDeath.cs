using UnityEngine;
using UnityEngine.Events;

namespace FridayNightFunkin.GamePlay
{
    public class PlayerDeath : MonoBehaviour
    {
        public UnityEvent OnPlayerDead;

        public UnityEvent OnGameOver;

        public UnityEvent OnGameOverEnd;

        private void Awake()
        {
            OnPlayerDead.AddListener(OnPlayerisDead);
        }

        private void OnDisable()
        {
            OnPlayerDead.RemoveListener(OnPlayerisDead);
        }

        public void OnPlayerisDead()
        {
            InputManager.inputActions.PlayableArrow.Disable();
        }
    }
}