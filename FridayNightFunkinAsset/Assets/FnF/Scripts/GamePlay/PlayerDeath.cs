using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace FridayNightFunkin.CHARACTERS
{
    public class PlayerDeath : MonoBehaviour,IService
    {
        public UnityEvent OnPlayerDead;

        public UnityEvent OnGameOver;

        public UnityEvent OnGameOverEnd;

        private ServiceLocator serviceLocator => ServiceLocator.instance;

        private void Awake()
        {
            serviceLocator.Register<PlayerDeath>(this);
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