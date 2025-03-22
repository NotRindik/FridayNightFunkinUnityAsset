using FridayNightFunkin.Editor.TimeLineEditor;
using FridayNightFunkin.GamePlay;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FridayNightFunkin.CHARACTERS
{
    public class Character_Fnf_Enemy : Character_FNF
    {

        private float arrowDetectRadiusCalcualted;

        internal CharacterSide characterSide = CharacterSide.Enemy;

        Coroutine coroutineIdle;

        public override RoadSide roadSide => RoadSide.Enemy;

        private ArrowTaker[] arrowTakers => chartPlayBack.arrowTakerEnemy;

        public override void Init()
        {
            base.Init();
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
            animator.CrossFade(SING_NOTES[(int)arrowSide], 0f, -1, 0f);
            StartCoroutine(RepearHitAnim(arrowSide));
        }

        public IEnumerator RepearHitAnim(ArrowSide arrowSide)
        {
            yield return new WaitForEndOfFrame();
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float clipLength = stateInfo.length;
            while(((ArrowTakerEnemy)arrowTakers[(int)arrowSide]).isHold)
            {
                animator.CrossFade(SING_NOTES[(int)arrowSide], 0f, -1, 0f);
                yield return new WaitForSeconds(clipLength);
            }
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