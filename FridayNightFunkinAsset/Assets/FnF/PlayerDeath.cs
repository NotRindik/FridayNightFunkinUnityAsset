using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        }
    }
}