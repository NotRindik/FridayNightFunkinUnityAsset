using FridayNightFunkin.UI;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace FridayNightFunkin
{
    public class ArrowTakerEnemy : ArrowTaker
    {
        private float distanceFromArrowToTaker;

        [SerializeField] private float timeToIdle = 0.3f;

        public delegate void OnArrowTakeHandler(ArrowSide arrow);
        public event OnArrowTakeHandler OnArrowTake;

        public delegate void OnArrowUnTakeHandler(ArrowSide arrow);
        public event OnArrowUnTakeHandler OnArrowUnTake;

        public bool isHold { get; private set; }

        private void Update()
        {
            foreach (var arrow in levelSettings.arrowsList)
            {
                if (arrow.arrowSide != arrowSide || !arrow.isWork || !arrow.isActiveAndEnabled)
                    continue;

                var distance = arrow.endPos.y - arrow.transform.position.y;
                if (arrow.transform.position.x == transform.position.x && distance < 0)
                {
                    animator.CrossFade("Pressed", 0);

                    isHold = false;
                    OnArrowTake?.Invoke(arrowSide);
                    if (arrow.tailDistance > 0)
                    {
                        isHold = true;
                        ArrowMask.instance.ActivateMask((int)arrowSide, CharacterSide.Enemy);
                        arrow.TakeArrow(isHold);
                        StartCoroutine(PlayAnimWithDelayWithCondition("Idle", timeToIdle, arrow));
                        break;
                    }
                    StartCoroutine(PlayAnimWithDelay("Idle", timeToIdle));
                    arrow.isWork = false;
                    arrow.TakeArrow(isHold);
                }
            }
        }

        private IEnumerator PlayAnimWithDelay(string anim,float time)
        {
            yield return new WaitForSeconds(time);
            animator.CrossFade(anim, 0);
            OnArrowUnTake?.Invoke(arrowSide);
        }

        private IEnumerator PlayAnimWithDelayWithCondition(string anim, float time,Arrow arrow)
        {
            while (true)
            {
                yield return new FixedUpdate();
                if (arrow.tailDistanceToArrowTakerRaw < 0)
                {
                    yield return new WaitForSeconds(time);
                    OnArrowUnTake?.Invoke(arrowSide);
                    animator.CrossFade(anim, 0);
                    break;
                }
            }

        }

        protected override void OnDrawGizmos()
        {
            DrawDetectRadius(Color.red);
        }
    }
}