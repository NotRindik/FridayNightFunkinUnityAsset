using FridayNightFunkin.GamePlay;
using UnityEngine;
using UnityEngine.Events;

namespace FridayNightFunkin.UI
{
    public class MenuBehaviour : MonoBehaviour
    {
        [SerializeField] protected UnityEvent OnEscapePressed;
        protected FnfInput inputActions;

        private void Start()
        {
            inputActions = InputManager.inputActions;
            inputActions.Enable();
        }
        protected virtual void Update()
        {
            if (inputActions.MenuNavigation.Escape.WasPerformedThisFrame())
            {
                Escape();
            }
        }
        public virtual void Escape()
        {
            OnEscapePressed?.Invoke();
        }
    }
}