using DIALOGUE;
using FridayNightFunkin.CHARACTERS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimPerBeat : MonoBehaviour
{
    [SerializeField]protected float BPM;
    protected float BPS;

    protected float time;
    protected Animator animator;

    private string beatAnimation = "Idle";

    private bool isPause;

    private bool isPlayer;

    private Character_Fnf_PlayAble player;
    protected void Awake()
    {
        BPS = BPM / 60;
        animator = GetComponent<Animator>();
        isPlayer = TryGetComponent(out Character_Fnf_PlayAble character_Fnf_Play);
        player = character_Fnf_Play;
        GameStateManager.instance.OnGameStateChanged += OnGameStateChanged;
    }
    protected void Update()
    {
        if(!isPause) time += Time.deltaTime;
        PlayAnimation();
    }

    private void OnGameStateChanged(GameState currentState)
    {
        isPause = currentState == GameState.Paused ? true : false;
    }

    public void SetPause(bool isPause)
    {
        this.isPause = isPause;
    }

    public void ChangeAnimation(string animation)
    {
        beatAnimation = animation;
    }
    public void ChangeBPM(int bpm)
    {
        BPM = bpm;
        BPS = BPM / 60;
    }

    protected virtual void PlayAnimation()
    {
        if (isPlayer)
        {
            if (time >= 1 / BPS /*&& !player.isHold*/)
            {
                time = 0;
                animator.Play(beatAnimation);
            }
        }
        else
        {
            if (time >= 1 / BPS)
            {
                time = 0;
                animator.Play(beatAnimation);
            }
        }
    }
}
