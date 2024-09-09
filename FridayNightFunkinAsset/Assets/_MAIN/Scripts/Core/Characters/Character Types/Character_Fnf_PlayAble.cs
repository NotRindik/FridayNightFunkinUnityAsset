using FridayNightFunkin.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace FridayNightFunkin.CHARACTERS
{
    public class Character_Fnf_PlayAble : Ñharacter_FNF
    {
        private string[] MiSS_ANIMATION = { "LeftFail", "DownFail", "UpFail", "DownFail" };

        [SerializeField] private UnityEvent OnPlayerDead;

        [SerializeField] private UnityEvent OnGameOver;

        [SerializeField] private UnityEvent OnGameOverEnd;

        private PlayAnimPerBeat playAnimPerBeat;

        private bool isDead = false;

        private bool isAnimationStart = false;

        private float timeToEndAnimation;

        private FnfInput inputActions;
        public LevelSettings levelSettings => LevelSettings.instance;

        private ArrowTakerPlayer[] arrowTakers;

        private float lockIdle;

        private void Start()
        {
            levelSettings.player = this;
            playAnimPerBeat = GetComponent<PlayAnimPerBeat>();
            arrowTakers = new ArrowTakerPlayer[levelSettings.arrowsPlayer.Length];
            for (int i = 0; i < levelSettings.arrowsPlayer.Length; i++)
            {
                arrowTakers[i] = levelSettings.arrowsPlayer[i].GetComponent<ArrowTakerPlayer>();
                arrowTakers[i].OnArrowTake += PlayHitAnimation;
                arrowTakers[i].OnArrowUnTake += PlayIdle;
            }
            InitializeActions();
        }
        private void InitializeActions()
        {
            inputActions = InputManager.inputActions;
        }

        private void Update()
        {
            if (inputActions.MenuNavigation.RestartAfterDie.WasPressedThisFrame() && isDead)
            {
                animator.Play("NotDead");
            }
            else if (inputActions.MenuNavigation.Escape.WasPressedThisFrame() && isDead)
            {
                PlayerPrefs.SetInt("AfterLevel",1);
                SceneLoad.instance.StartLoad("MainMenu");
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("NotDead") && !isAnimationStart)
            {
                timeToEndAnimation = animator.GetCurrentAnimatorStateInfo(0).length + 1.3f;
                OnGameOverEnd?.Invoke();
                playAnimPerBeat.SetPause(true);
                isAnimationStart = true;
            }

            if (timeToEndAnimation <= 0 && isAnimationStart)
            {
                isAnimationStart = false;
                SceneLoad.instance.StartLoad(SceneManager.GetActiveScene().name);
            }
        }
        private void FixedUpdate()
        {
            timeToEndAnimation -= Time.fixedDeltaTime;
            lockIdle -= Time.fixedDeltaTime;
        }

        public void DeathAnimEnd()
        {
            OnGameOver?.Invoke();
            playAnimPerBeat.SetPause(false);
            playAnimPerBeat.ChangeAnimation("DeadIdle");
        }

        private void PlayHitAnimation(ArrowSide arrowSide)
        {
            playAnimPerBeat.SetBlock(true);
            animator.CrossFade(SING_NOTES[(int)arrowSide],0.1f);
        }
        private void PlayIdle(ArrowSide arrowSide)
        {
            foreach (var arrowTaker in arrowTakers)
            {
                if (arrowTaker.isHold)
                {
                    return;
                }
            }
            playAnimPerBeat.SetBlock(false);
            animator.CrossFade(IDLE, 0.1f);
        }

        public void PlayMissAnimation(Arrow arrow)
        {
            animator.CrossFade(MiSS_ANIMATION[(int)arrow.arrowSide], 0);
            playAnimPerBeat.SetBlock(true);
            playAnimPerBeat.SetBlockTimer(false, 0.2f);

            if (FNFUIElement.instance.versusSlider.value == FNFUIElement.instance.versusSlider.minValue)
            {
                OnPlayerDead?.Invoke();
                playAnimPerBeat.SetPause(true);
                isDead = true;
                animator.Play("Dead");
            }
        }
    }
}