using FnF.Scripts.Extensions;
using FridayNightFunkin.Calculations;
using FridayNightFunkin.Editor;
using FridayNightFunkin.Editor.TimeLineEditor;
using UnityEngine;

namespace FridayNightFunkin.GamePlay
{
    public class PlayerMissTaker : ArrowSwitch
    {
        public PlayerMissTaker(ChartPlayBack chartPlayback) : base(chartPlayback)
        {
        }

        private StatisticManager StatisticManager => G.Instance.Get<StatisticManager>();

        public override void OnUpdate()
        {
            float camHeight = 2f * mainCamera.orthographicSize;
            float camWidth = camHeight * mainCamera.aspect;

            detectSize = new Vector2(camWidth, camHeight) + new Vector2(size * (mainCamera.orthographicSize / 5), size * (mainCamera.orthographicSize / 5));
            if (ChartPlayback.chartContainer.arrowsList.ContainsKey(RoadSide.Player))
            {
                foreach (var arrow in ChartPlayback.chartContainer.arrowsList[RoadSide.Player])
                {
                    if (IsArrowInsideCube(arrow.transform.position, canvasPosition.position, detectSize) && arrow.isWork)
                    {
                        arrow.gameObject.SetActive(true);
                        arrow.isViewedOnce = true;
                    }
                    else if (arrow.isViewedOnce && !IsArrowInsideCube(arrow.transform.position, canvasPosition.position, detectSize) && arrow.isWork)
                    {
                        if (arrow.distanceCount > 0)
                        {
                            if (IsArrowInsideCube(arrow.Tail.transform.position, canvasPosition.position, detectSize, true))
                            {
                                continue;
                            }
                        }
                        arrow.isWork = false;
                        arrow.gameObject.SetActive(false);
                        G.Instance.Get<HealthBar>().ModifyValue(-ChartPlayback.levelData.stage[ChartPlayBack.CurrentStageIndex].GetMissForce());
                        StatisticManager.AddMiss();
                        AudioManager.instance.PlaySoundEffect($"{FilePaths.resources_sfx}missnote{Random.Range(1, 4)}");
                        StatisticManager.CalculateAccuracy(500);
                        StatisticManager.CalculateTotalAccuracy(StatisticManager.accuracyList);
                        StatisticManager.ResetCombo();
                    }
                }
            }

        }
        private void OnDrawGizmos()
        {
            if (!canvasPosition) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(canvasPosition.position, detectSize);
        }
    }
}