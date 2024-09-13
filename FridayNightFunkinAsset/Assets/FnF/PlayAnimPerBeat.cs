using FridayNightFunkin;
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class PlayAnimPerBeat : MonoBehaviour
{
    [SerializeField]protected float ownBPM;
    protected float BPS;

    protected float time;
    protected Animator animator;

    private string beatAnimation = "Idle";

    private bool isPause;

    private bool isPlayer;

    private bool isBlock;
    protected void Awake()
    {
        GameStateManager.instance.OnGameStateChanged += OnGameStateChanged;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        if (ownBPM == 0)
            ownBPM = LevelSettings.instance.stage[LevelSettings.instance.stageIndex].GetGeneralBPM();
        BPS = ownBPM / 60;
    }
    protected void Update()
    {

        if (!isPause) 
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
        ownBPM = bpm;
        BPS = ownBPM / 60;
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
