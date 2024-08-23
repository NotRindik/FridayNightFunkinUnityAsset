using DIALOGUE;
using FridayNightFunkin.CHARACTERS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimPerBeat : MonoBehaviour
{
    [SerializeField]protected float BPM;
    protected float BPS => BPM / 60;

    protected float time;
    protected Animator animator;

    private bool isPlayer;
    protected void Awake()
    {
        animator = GetComponent<Animator>();
        isPlayer = TryGetComponent(out Character_Fnf_PlayAble character_Fnf_Play);
    }
    protected void Update()
    {
        time += Time.deltaTime;
        PlayAnimation();
    }

    protected virtual void PlayAnimation()
    {
        if (isPlayer)
        {
            if (time >= 1 / BPS && !PlayerInputManager.instance.isHold)
            {
                time = 0;
                animator.Play("Idle");
            }
        }
        else
        {
            if (time >= 1 / BPS)
            {
                time = 0;
                animator.Play("Idle");
            }
        }
    }
}
