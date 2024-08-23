using FridayNightFunkin.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FridayNightFunkin
{
    [ExecuteAlways]
    public class TurnOnArrowOnCameraView : MonoBehaviour
    {
        public LayerMask arrowLayer;
        private Vector2 detectSize;
        public float size;
        public Transform canvasPosition;
        private LevelSettings levelSettings => LevelSettings.instance;
        private ScoreManager scoreManager => ScoreManager.instance;
        public Camera mainCamera;

        private void OnDrawGizmos()
        {
            if (!canvasPosition || !gameObject.activeInHierarchy) return;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(canvasPosition.position, detectSize);
        }

        private void Update()
        {
            mainCamera = Camera.main;
            float camHeight = 2f * mainCamera.orthographicSize;
            float camWidth = camHeight * mainCamera.aspect;

            detectSize = new Vector2(camWidth, camHeight) + new Vector2(size * (mainCamera.orthographicSize/5), size * (mainCamera.orthographicSize / 5));
            if (!levelSettings)
                return;

            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                for (int i = 0; i < levelSettings.arrowsList.Count; i++)
                {
                    Arrow arrow = levelSettings.arrowsList[i];

                    if (IsPointInsideCube(arrow.transform.position, canvasPosition.position, detectSize) && arrow.isWork)
                    {
                        arrow.gameObject.SetActive(true);
                    }
                    else if(arrow.isActiveAndEnabled)
                    {
                        if (arrow.distanceCount == 0)
                        {
                            arrow.gameObject.SetActive(false);
                        }
                        if (arrow.tail)
                        {
                            if (!IsPointInsideCube(arrow.tail.transform.position, canvasPosition.position, detectSize))
                            {
                                arrow.gameObject.SetActive(false);
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < levelSettings.arrowsList.Count; i++)
                {
                    Arrow arrow = levelSettings.arrowsList[i];

                    if (IsPointInsideCube(arrow.transform.position, canvasPosition.position, detectSize) && arrow.isWork)
                    {
                        arrow.gameObject.SetActive(true);
                        arrow.isViewedOnce = true;
                    }
                    else if(arrow.isViewedOnce && !IsPointInsideCube(arrow.transform.position, canvasPosition.position, detectSize))
                    {
                        if (arrow.distanceCount == 0 && arrow.isActiveAndEnabled)
                        {
                            arrow.gameObject.SetActive(false);
                            levelSettings.player.PlayMissAnimation(arrow);
                            scoreManager.ReduceValueToSlider(levelSettings.missForce);
                            scoreManager.AddMiss();
                            scoreManager.ÑalculateAccuracy(1);
                            scoreManager.ÑalculateTotalAccuracy(scoreManager.accuracyList);
                            scoreManager.ResetCombo();
                        }
                        if (arrow.tail && arrow.tail.gameObject.activeInHierarchy)
                        {
                            if (!IsPointInsideCube(arrow.tail.transform.position, canvasPosition.position, detectSize))
                            {
                                arrow.gameObject.SetActive(false);
                                levelSettings.player.PlayMissAnimation(arrow);
                                scoreManager.ReduceValueToSlider(levelSettings.missForce);
                                scoreManager.AddMiss();
                                scoreManager.ÑalculateAccuracy(1);
                                scoreManager.ÑalculateTotalAccuracy(scoreManager.accuracyList);
                                scoreManager.ResetCombo();
                            }
                        }
                    }
                }
            }

        }
        bool IsPointInsideCube(Vector3 point, Vector3 cubeCenter, Vector3 cubeSize)
        {
            Vector3 minPoint = cubeCenter - cubeSize / 2;
            Vector3 maxPoint = cubeCenter + cubeSize / 2;
            return point.x >= minPoint.x && point.x <= maxPoint.x &&
                   point.y >= minPoint.y && point.y <= maxPoint.y;
        }
    }
}