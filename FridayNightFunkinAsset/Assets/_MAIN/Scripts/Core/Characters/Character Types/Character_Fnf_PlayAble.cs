using FridayNightFunkin.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;

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

        public Character_Fnf_PlayAble character;
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
            if (Input.anyKeyDown && isDead)
            {
                animator.Play("NotDead");
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("NotDead") && !isAnimationStart)
            {
                timeToEndAnimation = animator.GetCurrentAnimatorStateInfo(0).length + 1;
                OnGameOverEnd?.Invoke();
                playAnimPerBeat.SetPause(true);
                isAnimationStart = true;
            }

            if (timeToEndAnimation <= 0 && isAnimationStart)
            {
                isAnimationStart = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

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

            SetIdleAnim(0.4f);

            if (FNFUIElement.instance.versusSlider.value == FNFUIElement.instance.versusSlider.minValue)
            {
                OnPlayerDead?.Invoke();
                playAnimPerBeat.SetPause(true);
                isDead = true;
                animator.Play("Dead");
            }
        }

        public void SetIdleAnim(float time)
        {
            lockIdle = time;
        }
    }
}