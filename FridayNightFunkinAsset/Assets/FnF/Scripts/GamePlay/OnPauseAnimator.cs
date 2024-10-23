using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPauseAnimator : OnGameStateChange
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    protected override void OnGameStateChanged(GameState currenState)
    {
        if (currenState == GameState.Paused)
        {
            animator.speed = 0;
        }
        else
        {
            animator.speed = 1;
        }
    }
}
