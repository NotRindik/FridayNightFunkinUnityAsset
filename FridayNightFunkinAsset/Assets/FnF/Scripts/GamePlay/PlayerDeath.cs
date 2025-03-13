using System.Collections.Generic;
using FnF.Scripts.Extensions;
using FridayNightFunkin.CHARACTERS;
using FridayNightFunkin.Editor.TimeLineEditor;
using UnityEngine;
using UnityEngine.Events;

namespace FridayNightFunkin.GamePlay
{
    public class PlayerDeath : MonoBehaviour, IService
    {
        public UnityEvent OnPlayerDead;

        public UnityEvent OnGameOver;

        public UnityEvent OnGameOverEnd;

        public void Init()
        {
            OnPlayerDead.AddListener(OnPlayerisDead);
        }

        private void OnDisable()
        {
            OnPlayerDead.RemoveListener(OnPlayerisDead);
        }

        public void OnPlayerisDead()
        {
            var players = G.Instance.Get<CharacterSpawner>().currentPlayer;
            SetSortingOrder(players, 801);
            
            InputManager.inputActions.PlayableArrow.Disable();
        }
        
        private void SetSortingOrder(IEnumerable<Character_FNF> chars, int sortingOrder)
        {
            foreach (var c in chars)
            {
                ComponentFinder.FindComponentAndCheckChilds<SpriteRenderer>(c.gameObject).sortingOrder = sortingOrder;
            }
        }
    }
}