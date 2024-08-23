using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        if (inputActions.MenuNavigation.Escape.WasPressedThisFrame())
        {
            Escape();
        }
    }
    protected virtual void Escape()
    {
        OnEscapePressed?.Invoke();
    }
}
