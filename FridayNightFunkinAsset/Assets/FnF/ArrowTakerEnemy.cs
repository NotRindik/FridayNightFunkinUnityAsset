using FridayNightFunkin.UI;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace FridayNightFunkin
{
    public class ArrowTakerEnemy : ArrowTaker
    {
        private float distanceFromArrowToTaker;

        [SerializeField] private float timeToIdle = 0.1f;

        public delegate void OnArrowTakeHandler(ArrowSide arrow);
        public event OnArrowTakeHandler OnArrowTake;

        public delegate void OnArrowUnTakeHandler(ArrowSide arrow);
        public event OnArrowUnTakeHandler OnArrowUnTake;

        public bool isHold { get; private set; }

        private int isDownScroll;

        private void Start()
        {
            isDownScroll = ServiceLocator.instance.Get<ChangesByGameSettings>().downscroll;
        }

        private void Update()
        {
            foreach (var arrow in levelSettings.arrowsList)
            {
                if (arrow.arrowSide != arrowSide || !arrow.isWork || !arrow.isActiveAndEnabled || arrow.characterSide != CharacterSide.Enemy)
                    continue;

                var distance = (Camera.main.WorldToScreenPoint(arrow.endPos).y - Camera.main.WorldToScreenPoint(arrow.transform.position).y) * (isDownScroll == 1? -1 : 1);
                if (distance < 0)
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
                    ScoreManager.instance.ReduceValueToSliderEnemy(LevelSettings.instance.stage[LevelSettings.instance.stageIndex].GetEnemyForce());
                    FNFUIElement.instance.UpdateUI();
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
                    isHold = false;
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