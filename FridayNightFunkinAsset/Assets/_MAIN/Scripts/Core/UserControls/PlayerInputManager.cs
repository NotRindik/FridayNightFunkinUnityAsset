using FridayNightFunkin;
using FridayNightFunkin.CHARACTERS;
using FridayNightFunkin.UI;
using History;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DIALOGUE
{
    public class PlayerInputManager : MonoBehaviour
    {
        private PlayerInput input;

        private List<(InputAction action, Action<InputAction.CallbackContext> command)> actions = new List<(InputAction action, Action<InputAction.CallbackContext> command)>();

        public static PlayerInputManager instance;

        void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            input = GetComponent<PlayerInput>();
        }

        public void OnNext(InputAction.CallbackContext c)
        {
            DialogueSystem.instance.OnUserPromt_Next();
        }

        public void OnHistoryBack(InputAction.CallbackContext c)
        {
            HistoryManager.instance.GoBack();
        }

        public void OnHistoryForward(InputAction.CallbackContext c)
        {
            HistoryManager.instance.GoForward();
        }

        public void OnHistoryToggleLog(InputAction.CallbackContext c)
        {
            var logs = HistoryManager.instance.logManager;
            if (!logs.isOpen)
                logs.Open();
            else
                logs.Close();
        }
    }
}