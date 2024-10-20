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

        private PlayAnimPerBeat playAnimPerBeat;

        private bool isDead = false;

        private bool isAnimationStart = false;

        private float timeToEndAnimation;

        private FnfInput inputActions;
        public LevelSettings levelSettings => LevelSettings.instance;

        private ArrowTakerPlayer[] arrowTakers;

        private float lockIdle;

        internal CharacterSide characterSide = CharacterSide.Player;

        private PlayerDeath playerDeath;

        private bool isRestartPressed;

        public bool isActive { get; private set; }

        private Coroutine startIdleDelay;


        protected override void Awake()
        {
            base.Awake();
            if (TryGetComponent(out PlayAnimPerBeat playAnimPerBeat))
            {
                this.playAnimPerBeat = playAnimPerBeat;
            }
            else
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).TryGetComponent(out PlayAnimPerBeat playAnimPerBeatChild))
                    {
                        this.playAnimPerBeat = playAnimPerBeatChild;
                    }
                    else
                    {
                        Debug.LogWarning($"No playAnimPerBeat {gameObject}");
                    }
                }
            }
        }

        public void Active()
        {
            for (int i = 0; i < levelSettings.arrowsPlayer.Length; i++)
            {
                arrowTakers[i] = levelSettings.arrowsPlayer[i].GetComponent<ArrowTakerPlayer>();
                arrowTakers[i].OnArrowTake += PlayHitAnimation;
                arrowTakers[i].OnArrowUnTake += PlayIdle;
            }
            isActive = true;
        }
        public void Disactive()
        {
            for (int i = 0; i < levelSettings.arrowsPlayer.Length; i++)
            {
                arrowTakers[i] = levelSettings.arrowsPlayer[i].GetComponent<ArrowTakerPlayer>();
                arrowTakers[i].OnArrowTake -= PlayHitAnimation;
                arrowTakers[i].OnArrowUnTake -= PlayIdle;
            }
            isActive = false;
            ReloadAnim();
        }
        private void Start()
        {
            inputActions = InputManager.inputActions;
            arrowTakers = new ArrowTakerPlayer[levelSettings.arrowsPlayer.Length];
            playerDeath = ServiceLocator.instance.Get<PlayerDeath>();
            Active();
        }

        private void Update()
        {
            if (inputActions.MenuNavigation.RestartAfterDie.WasPressedThisFrame() && isDead && !isRestartPressed)
            {
                animator.Play("NotDead");
                isRestartPressed = true;
            }
            else if (inputActions.MenuNavigation.Escape.WasPressedThisFrame() && isDead)
            {
                PlayerPrefs.SetInt("AfterLevel",1);
                SceneLoad.instance.StartLoad("MainMenu");
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("NotDead") && !isAnimationStart)
            {
                timeToEndAnimation = animator.GetCurrentAnimatorStateInfo(0).length + 1.3f;
                playerDeath.OnGameOverEnd?.Invoke();
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
            playerDeath.OnGameOver?.Invoke();
            playAnimPerBeat.SetPause(false);
            playAnimPerBeat.ChangeAnimation("DeadIdle");
        }

        private void PlayHitAnimation(ArrowSide arrowSide)
        {
            if (!isActive)
                return;
            playAnimPerBeat.SetBlock(true);
            animator.CrossFade(SING_NOTES[(int)arrowSide],0.1f);
        }

        private void ReloadAnim()
        {
            playAnimPerBeat.SetBlock(false);
            animator.CrossFade(IDLE, 0.1f);
        }
        
        private void PlayIdle(ArrowSide arrowSide)
        {
            if (!isActive)
                return;
            StartIdleDelay();
        }

        public void StartIdleDelay()
        {
            if(startIdleDelay != null)
            {
                StopCoroutine(startIdleDelay);
            }

            startIdleDelay = StartCoroutine(IdleDelay(0.5f));
        }

        public IEnumerator IdleDelay(float a)
        {
            yield return new WaitForSeconds(a);
            foreach (var arrowTaker in arrowTakers)
            {
                if (arrowTaker.isHold)
                {
                    yield break;
                }
            }

            playAnimPerBeat.SetBlock(false);
            animator.CrossFade(IDLE, 0.1f);
        }
        public void PlayMissAnimation(Arrow arrow)
        {
            if (FNFUIElement.instance.versusSlider.value == FNFUIElement.instance.versusSlider.minValue)
            {
                playAnimPerBeat.SetPause(true);
                isDead = true;
                animator.CrossFade("Dead", 0);
                StartCoroutine(DeathAnimationEnd(2.1f));
                playerDeath.OnPlayerDead?.Invoke();
                return;
            }

            animator.CrossFade(MiSS_ANIMATION[(int)arrow.arrowSide], 0);
            playAnimPerBeat.SetBlock(true);
            playAnimPerBeat.SetBlockTimer(false, 0.2f);
        }

        private IEnumerator DeathAnimationEnd(float time)
        {
            yield return new WaitForSeconds(time);
            DeathAnimEnd();
        }
    }
}