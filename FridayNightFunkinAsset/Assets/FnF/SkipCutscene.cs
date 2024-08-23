using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SkipCutscene : MonoBehaviour
{
    [SerializeField] private PlayableDirector cutscene;
    [SerializeField] private float skipTime = 0;
    private FnfInput inputActions;

    private void OnEnable()
    {
        inputActions = InputManager.inputActions;
        inputActions.Enable();
        inputActions.MenuNavigation.SkipCutscene.performed += context => SkipTheCutscene();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }


    private void SkipTheCutscene()
    {
        if(skipTime == 0)
        {
            cutscene.time = cutscene.duration;
        }
        else
        {
            cutscene.time = skipTime;
        }
    }
}
