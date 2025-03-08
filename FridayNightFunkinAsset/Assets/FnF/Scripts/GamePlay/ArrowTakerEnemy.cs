using FridayNightFunkin.Calculations;
using FridayNightFunkin.Settings;
using FridayNightFunkin.UI;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace FridayNightFunkin.GamePlay
{
    public class ArrowTakerEnemy : ArrowTaker
    {
        private float distanceFromArrowToTaker;

        [SerializeField] private float timeToIdle = 0.1f;

        public bool isHold { get; private set; }

        private int isDownScroll;
        public override RoadSide roadSide => RoadSide.Enemy;

        private void Start()
        {
            isDownScroll = ChangesByGameSettings.instance.downscroll;
        }

        private void Update()
        {
            foreach (var arrow in chartPlayBack.ChartContainer.arrowsList[roadSide])
            {
                if (arrow.arrowSide != arrowSide || !arrow.isWork || !arrow.isActiveAndEnabled)
                    continue;

                var distance = (Camera.WorldToScreenPoint(arrow.endPos).y - Camera.WorldToScreenPoint(arrow.transform.position).y) * (isDownScroll == 1? -1 : 1);
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
                    ScoreManager.instance.ReduceValueToSliderEnemy(chartPlayBack.levelData.stage[chartPlayBack.currentStageIndex].GetEnemyForce());
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