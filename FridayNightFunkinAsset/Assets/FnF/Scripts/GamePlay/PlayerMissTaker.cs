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

        private ScoreManager scoreManager => ScoreManager.instance;

        public override void OnUpdate()
        {
            float camHeight = 2f * mainCamera.orthographicSize;
            float camWidth = camHeight * mainCamera.aspect;

            detectSize = new Vector2(camWidth, camHeight) + new Vector2(size * (mainCamera.orthographicSize / 5), size * (mainCamera.orthographicSize / 5));

            foreach (var arrow in chartPlayback.arrowsList[RoadSide.Player])
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
                        if (IsArrowInsideCube(arrow.tail.transform.position, canvasPosition.position, detectSize,true))
                        {
                            continue;
                        }
                    }
                    arrow.isWork = false;
                    arrow.gameObject.SetActive(false);
                    scoreManager.ReduceValueToSlider(chartPlayback.levelData.stage[chartPlayback.currentStageIndex].GetMissForce());
                    scoreManager.AddMiss();
                    AudioManager.instance.PlaySoundEffect($"{FilePaths.resources_sfx}missnote{Random.Range(1,4)}");
                    scoreManager.ÑalculateAccuracy(500);
                    scoreManager.ÑalculateTotalAccuracy(scoreManager.accuracyList);
                    scoreManager.ResetCombo();
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