using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FridayNightFunkin
{
    public class PlayerMissTaker : MonoBehaviour
    {
        [SerializeField] private float size;
        [SerializeField] private Transform canvasPosition;
        private LevelSettings levelSettings => LevelSettings.instance;
        private ScoreManager scoreManager => ScoreManager.instance;
        private Vector2 detectSize;
        private Camera mainCamera => Camera.main;

        void Update()
        {
            float camHeight = 2f * mainCamera.orthographicSize;
            float camWidth = camHeight * mainCamera.aspect;

            detectSize = new Vector2(camWidth, camHeight) + new Vector2(size * (mainCamera.orthographicSize / 5), size * (mainCamera.orthographicSize / 5));

            foreach (var arrow in levelSettings.arrowsList)
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
                    foreach (var currentPlayer in levelSettings.currentPlayer)
                    {
                        if(currentPlayer.isActive)
                            currentPlayer.PlayMissAnimation(arrow);
                    }
                    scoreManager.ReduceValueToSlider(levelSettings.stage[levelSettings.stageIndex].GetMissForce());
                    scoreManager.AddMiss();
                    scoreManager.ÑalculateAccuracy(500);
                    scoreManager.ÑalculateTotalAccuracy(scoreManager.accuracyList);
                    scoreManager.ResetCombo();
                }
            }

        }
        private void OnDrawGizmos()
        {
            if (!canvasPosition || !gameObject.activeInHierarchy) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(canvasPosition.position, detectSize);
        }


        private bool IsArrowInsideCube(Vector3 point, Vector3 cubeCenter, Vector3 cubeSize, bool istail = false)
        {
            Vector3 minPoint = cubeCenter - cubeSize / 2;
            Vector3 maxPoint = cubeCenter + cubeSize / 2;
            if (!istail)
            {
                return point.x >= minPoint.x && point.x <= maxPoint.x &&
                       point.y >= minPoint.y && point.y <= maxPoint.y;
            }
            else
            {
                return point.x >= minPoint.x && point.x <= maxPoint.x && point.y <= maxPoint.y;
            }
        }
    }
}