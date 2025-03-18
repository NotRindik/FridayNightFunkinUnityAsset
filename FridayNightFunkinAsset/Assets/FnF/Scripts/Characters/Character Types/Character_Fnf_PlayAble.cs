using FridayNightFunkin.Editor.TimeLineEditor;
using FridayNightFunkin.GamePlay;
using FridayNightFunkin.Settings;
using FridayNightFunkin.UI;
using System.Collections;
using System.Collections.Generic;
using FnF.Scripts.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace FridayNightFunkin.CHARACTERS
{
    public class Character_Fnf_PlayAble : Character_FNF
    {
        private string[] MiSS_ANIMATION = { "LeftFail", "DownFail", "UpFail", "DownFail" };

        private bool isDead = false;

        private bool isAnimationStart = false;

        private float timeToEndAnimation;

        private FnfInput inputActions;

        private float lockIdle;

        internal CharacterSide characterSide = CharacterSide.Player;

        private PlayerDeath playerDeath => G.Instance.Get<PlayerDeath>();

        private bool isRestartPressed;

        public bool isActive { get; private set; }

        private Coroutine startIdleDelay;

        public override RoadSide roadSide => RoadSide.Player;
        private IEnumerable<ArrowTaker> arrowTakers => chartPlayBack.arrowTakerPlayer;

        public void Activate()
        {
            foreach (var arrowTaker in arrowTakers)
            {
                ((ArrowTakerPlayer)arrowTaker).OnArrowTake += PlayHitAnimation;
                ((ArrowTakerPlayer)arrowTaker).OnArrowUnTake += PlayIdle;
            }
            isActive = true;
        }
        public void Disactive()
        {
            foreach (var arrowTaker in arrowTakers)
            {
                ((ArrowTakerPlayer)arrowTaker).OnArrowTake -= PlayHitAnimation;
                ((ArrowTakerPlayer)arrowTaker).OnArrowUnTake -= PlayIdle;
            }
            isActive= false;
            ReloadAnim();
        }
        private void Start()
        {
            inputActions = InputManager.InputActions;
            Activate();
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
                G.Instance.Get<SceneLoad>().StartLoad("MainMenu");
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
                G.Instance.Get<SceneLoad>().StartLoad(SceneManager.GetActiveScene().name);
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
                if (((ArrowTakerPlayer)arrowTaker).IsHold)
                {
                    yield break;
                }
            }

            playAnimPerBeat.SetBlock(false);
            animator.CrossFade(IDLE, 0.1f);
        }
        public void PlayMissAnimation(Arrow arrow)
        {
            var healthBar = G.Instance.Get<HealthBar>().healthBarData.healthBar;
            if (healthBar.value == healthBar.minValue)
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

        public void PlayMissAnimation(ArrowSide arrowSide)
        {
            var healthBar = G.Instance.Get<HealthBar>().healthBarData.healthBar;
            if (healthBar.value == healthBar.minValue)
            {
                playAnimPerBeat.SetPause(true);
                isDead = true;
                animator.CrossFade("Dead", 0);
                StartCoroutine(DeathAnimationEnd(2.1f));
                playerDeath.OnPlayerDead?.Invoke();
                return;
            }

            animator.CrossFade(MiSS_ANIMATION[(int)arrowSide], 0);
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