using System.Collections;
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

    private bool isBlock;
    protected void Awake()
    {
        BPS = BPM / 60;
        animator = GetComponent<Animator>();
        GameStateManager.instance.OnGameStateChanged += OnGameStateChanged;
    }
    protected void Update()
    {
        if(!isPause) 
            if(!isBlock) time += Time.deltaTime;
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

    public void SetBlockTimer(bool isBlock, float time)
    {
        StartCoroutine(SetBlockTimerCoroutine(isBlock, time));
    }

    private IEnumerator SetBlockTimerCoroutine(bool isBlock, float time)
    {
        yield return new WaitForSeconds(time);
        SetBlock(isBlock);
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

    public void SetBlock(bool setBlock)
    {
        isBlock = setBlock;
    }

    protected virtual void PlayAnimation()
    {

        if (time >= 1 / BPS)
        {
            time = 0;
            animator.Play(beatAnimation);
        }
    }
}
