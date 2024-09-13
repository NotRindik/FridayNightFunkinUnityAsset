using FridayNightFunkin.CHARACTERS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FridayNightFunkin.Service
{
    public class ServiceLocatorInGame : MonoBehaviour
    {
        [SerializeField] private PlayerDeath playerDeath;
        public void Awake()
        {
            ServiceLocator.instance.Register<PlayerDeath>(playerDeath);
        }
    }
}