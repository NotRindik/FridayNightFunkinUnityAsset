using FnF.Scripts.Extensions;
using FridayNightFunkin.GamePlay;
using UnityEngine;
using UnityEngine.Events;

namespace FridayNightFunkin.UI
{
    public class MenuBehaviour : MonoBehaviour,IService
    {
        [SerializeField] protected UnityEvent OnEscapePressed;
        protected FnfInput inputActions;

        protected virtual void Start()
        {
            inputActions = InputManager.InputActions;
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