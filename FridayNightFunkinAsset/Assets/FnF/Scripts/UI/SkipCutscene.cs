using FridayNightFunkin.GamePlay;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

namespace FridayNightFunkin.UI
{
    public class SkipCutscene : MonoBehaviour
    {
        [SerializeField] private PlayableDirector cutscene;
        [SerializeField] private float skipTime = 0;
        private FnfInput inputActions;

        private void OnEnable()
        {
            inputActions = InputManager.inputActions;
            inputActions.MenuNavigation.SkipCutscene.Enable();
            inputActions.MenuNavigation.SkipCutscene.performed += SkipTheCutscene;
        }
        private void OnDisable()
        {
            inputActions.MenuNavigation.SkipCutscene.performed -= SkipTheCutscene;
            inputActions.MenuNavigation.SkipCutscene.Disable();
        }

        private void OnDestroy()
        {
            inputActions.MenuNavigation.SkipCutscene.performed -= SkipTheCutscene;
            inputActions.MenuNavigation.SkipCutscene.Disable();
        }


        private void SkipTheCutscene(InputAction.CallbackContext callbackContext)
        {
            if (skipTime == 0)
            {
                cutscene.time = cutscene.duration;
            }
            else
            {
                cutscene.time = skipTime;
            }
        }
    }
}