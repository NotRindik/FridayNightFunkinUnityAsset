using System;
using FridayNightFunkin.Calculations;
using FridayNightFunkin.Settings;
using FridayNightFunkin.UI;
using System.Collections;
using FnF.Scripts.Extensions;
using FnF.Scripts.Settings;
using FridayNightFunkin.Editor.TimeLineEditor;
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
        public override RoadSide RoadSide => RoadSide.Enemy;
        

        protected override void Start()
        {
            base.Start();
            isDownScroll = G.Instance.Get<SettingsManager>().activeGameSettings.Downscroll;
        }

        private void Update()
        {
            if (chartPlayBack)
            {
                foreach (var arrow in chartPlayBack.chartContainer.arrowsList[RoadSide])
                {
                    if (arrow.arrowSide != arrowSide || !arrow.isWork || !arrow.isActiveAndEnabled || arrow == null)
                        continue;

                    var distance = (Camera.WorldToScreenPoint(arrow.EndPos).y - Camera.WorldToScreenPoint(arrow.transform.position).y) * (isDownScroll == 1 ? -1 : 1);
                    if (distance < 0)
                    {
                        Animator.CrossFade("Pressed", 0);

                        isHold = false;
                        OnArrowTake?.Invoke(arrowSide);
                        if (arrow.TailDistance > 0)
                        {
                            isHold = true;
                            ArrowMask.instance.ActivateMask((int)arrowSide, CharacterSide.Enemy);
                            arrow.TakeArrow(isHold);
                            StartCoroutine(PlayAnimWithDelayWithCondition("Idle", timeToIdle, arrow));
                            break;
                        }
                        G.Instance.Get<HealthBar>().ModifyValue(-chartPlayBack.levelData.stage[ChartPlayBack.CurrentStageIndex].GetEnemyForce());
                        StartCoroutine(PlayAnimWithDelay("Idle", timeToIdle));
                        arrow.isWork = false;
                        arrow.TakeArrow(isHold);
                    }
                }   
            }
        }

        private IEnumerator PlayAnimWithDelay(string anim,float time)
        {
            yield return new WaitForSeconds(time);
            Animator.CrossFade(anim, 0);
            OnArrowUnTake?.Invoke(arrowSide);
        }

        private IEnumerator PlayAnimWithDelayWithCondition(string anim, float time,Arrow arrow)
        {
            while (true)
            {
                yield return new FixedUpdate();
                if (arrow.TailDistanceToArrowTakerRaw < 0)
                {
                    yield return new WaitForSeconds(time);
                    isHold = false;
                    OnArrowUnTake?.Invoke(arrowSide);
                    Animator.CrossFade(anim, 0);
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