using FridayNightFunkin.Editor.TimeLineEditor;
using FridayNightFunkin.GamePlay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FridayNightFunkin.CHARACTERS
{
    public class Character_Fnf_Enemy : Ñharacter_FNF
    {
        private PlayAnimPerBeat playAnimPerBeat;

        private float arrowDetectRadiusCalcualted;

        internal CharacterSide characterSide = CharacterSide.Enemy;

        Coroutine coroutineIdle;

        public override RoadSide roadSide => RoadSide.Enemy;

        private IEnumerable<ArrowTaker> arrowTakers => chartPlayBack.arrowTakerEnemy;

        private void Start()
        {
            TryGetComponent(out PlayAnimPerBeat playAnimPerBeat);
            this.playAnimPerBeat = playAnimPerBeat;

            Activate();
        }
        public void Activate()
        {
            foreach (var arrowTaker in arrowTakers)
            {
                (arrowTaker as ArrowTakerEnemy).OnArrowTake += PlayHitAnimation;
                (arrowTaker as ArrowTakerEnemy).OnArrowUnTake += PlayIdle;
            }
        }
        private void ReloadAnim()
        {
            playAnimPerBeat.SetBlock(false);
            animator.CrossFade(IDLE, 0.1f);
        }
        public void Disactive()
        {
            foreach (var arrowTaker in arrowTakers)
            {
                (arrowTaker as ArrowTakerEnemy).OnArrowTake -= PlayHitAnimation;
                (arrowTaker as ArrowTakerEnemy).OnArrowUnTake -= PlayIdle;
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
                if ((arrowTaker as ArrowTakerEnemy).isHold)
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