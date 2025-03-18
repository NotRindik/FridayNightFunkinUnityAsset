using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
using TMPro;
namespace FridayNightFunkin.GamePlay
{
    public class InputManager : MonoBehaviour
    {
        public static FnfInput InputActions;

        public static event Action rebindComplete;
        public static event Action rebindCanceled;
        public static event Action<InputAction, int> rebindStarted;

        public static void StartRebind(string actionName, int bindingIndex, TextMeshProUGUI statusText, Image overlay, bool excludeMouse)
        {
            InputAction action = InputActions.asset.FindAction(actionName);
            if (action == null || action.bindings.Count <= bindingIndex)
            {
                Debug.Log("Couldn't find action or binding");
                return;
            }

            if (action.bindings[bindingIndex].isComposite)
            {
                var firstPartIndex = bindingIndex + 1;
                if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isComposite)
                    DoRebind(action, bindingIndex, statusText, overlay, true, excludeMouse);
            }
            else
                DoRebind(action, bindingIndex, statusText, overlay, false, excludeMouse);
        }

        private static void DoRebind(InputAction actionToRebind, int bindingIndex, TextMeshProUGUI statusText, Image overlay, bool allCompositeParts, bool excludeMouse)
        {
            if (actionToRebind == null || bindingIndex < 0)
                return;

            //statusText.text = $"Press a {actionToRebind.expectedControlType}";

            actionToRebind.Disable();

            var rebind = actionToRebind.PerformInteractiveRebinding(bindingIndex);

            overlay.gameObject.SetActive(true);

            rebind.OnComplete(operation =>
            {
                actionToRebind.Enable();
                operation.Dispose();

                if (allCompositeParts)
                {
                    var nextBindingIndex = bindingIndex + 1;
                    if (nextBindingIndex < actionToRebind.bindings.Count && actionToRebind.bindings[nextBindingIndex].isComposite)
                        DoRebind(actionToRebind, nextBindingIndex, statusText, overlay, allCompositeParts, excludeMouse);
                }

                SaveBindingOverride(actionToRebind);
                rebindComplete?.Invoke();
                overlay.gameObject.SetActive(false);
            });

            rebind.OnCancel(operation =>
            {
                actionToRebind.Enable();
                operation.Dispose();
                overlay.gameObject.SetActive(false);

                rebindCanceled?.Invoke();
            });

            rebind.WithCancelingThrough("<Keyboard>/escape");

            if (excludeMouse)
                rebind.WithControlsExcluding("Mouse");

            rebindStarted?.Invoke(actionToRebind, bindingIndex);
            rebind.Start(); //actually starts the rebinding process
        }

        public static string GetBindingName(string actionName, int bindingIndex)
        {
            if (InputActions == null)
                InputActions = new FnfInput();

            InputAction action = InputActions.asset.FindAction(actionName);
            return action.GetBindingDisplayString(bindingIndex);
        }

        private static void SaveBindingOverride(InputAction action)
        {
            for (int i = 0; i < action.bindings.Count; i++)
            {
                PlayerPrefs.SetString(action.actionMap + action.name + i, action.bindings[i].overridePath);
            }
        }

        public static void LoadBindingOverride(string actionName)
        {
            if (actionName == null)
                return;

            if (InputActions == null)
                InputActions = new FnfInput();

            InputAction action = InputActions.asset.FindAction(actionName);

            for (int i = 0; i < action.bindings.Count; i++)
            {
                if (!string.IsNullOrEmpty(PlayerPrefs.GetString(action.actionMap + action.name + i)))
                    action.ApplyBindingOverride(i, PlayerPrefs.GetString(action.actionMap + action.name + i));
            }
        }

        public static void ResetBinding(string actionName, int bindingIndex)
        {
            InputAction action = InputActions.asset.FindAction(actionName);

            if (action == null || action.bindings.Count <= bindingIndex)
            {
                Debug.Log("Could not find action or binding");
                return;
            }

            if (action.bindings[bindingIndex].isComposite)
            {
                for (int i = bindingIndex; i < action.bindings.Count && action.bindings[i].isComposite; i++)
                    action.RemoveBindingOverride(i);
            }
            else
                action.RemoveBindingOverride(bindingIndex);

            SaveBindingOverride(action);
        }

    }
}