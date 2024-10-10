using FridayNightFunkin.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace FridayNightFunkin.CHARACTERS
{
    public class Character_Fnf_Enemy : Ñharacter_FNF
    {
        private LevelSettings levelSettings => LevelSettings.instance;
        private ArrowTakerEnemy[] arrowTakers;
        private PlayAnimPerBeat playAnimPerBeat;

        private float arrowDetectRadiusCalcualted;

        internal CharacterSide characterSide = CharacterSide.Enemy;

        Coroutine coroutineIdle;

        private void Start()
        {
            arrowTakers = new ArrowTakerEnemy[levelSettings.arrowsEnemy.Length];
            TryGetComponent(out PlayAnimPerBeat playAnimPerBeat);
            this.playAnimPerBeat = playAnimPerBeat;
            for (int i = 0; i < levelSettings.arrowsEnemy.Length; i++)
            {
                arrowTakers[i] = levelSettings.arrowsEnemy[i].GetComponent<ArrowTakerEnemy>();
                arrowTakers[i].OnArrowTake += PlayHitAnimation;
                arrowTakers[i].OnArrowUnTake += PlayIdle;
            }
        }
        public void Active()
        {
            for (int i = 0; i < levelSettings.arrowsEnemy.Length; i++)
            {
                arrowTakers[i] = levelSettings.arrowsEnemy[i].GetComponent<ArrowTakerEnemy>();
                arrowTakers[i].OnArrowTake += PlayHitAnimation;
                arrowTakers[i].OnArrowUnTake += PlayIdle;
            }
        }
        private void ReloadAnim()
        {
            playAnimPerBeat.SetBlock(false);
            animator.CrossFade(IDLE, 0.1f);
        }
        public void Disactive()
        {
            for (int i = 0; i < levelSettings.arrowsEnemy.Length; i++)
            {
                arrowTakers[i] = levelSettings.arrowsEnemy[i].GetComponent<ArrowTakerEnemy>();
                arrowTakers[i].OnArrowTake -= PlayHitAnimation;
                arrowTakers[i].OnArrowUnTake -= PlayIdle;
            }
            ReloadAnim();
        }
        private void PlayHitAnimation(ArrowSide arrowSide)
        {
            playAnimPerBeat.SetBlock(true);
            animator.Play(SING_NOTES[(int)arrowSide]);
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
            StartAnimation();
        }

        public void StartAnimation()
        {
            StopAllCoroutines();
            StartCoroutine(playIdleInvoke());
        }

        private IEnumerator playIdleInvoke()
        {
            yield return new WaitForSeconds(0.5f);
            animator.CrossFade(IDLE, 0.1f);
        }
    }
}