using FridayNightFunkin.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FridayNightFunkin.Editor
{
    [ExecuteAlways]
    public class TurnOnArrowOnCameraView : MonoBehaviour
    {
        private Vector2 detectSize;
        public float size;
        public Transform canvasPosition;
        private LevelSettings levelSettings => LevelSettings.instance;
        private Camera mainCamera;

        private bool isUpdateWork = false;

        private void OnDrawGizmos()
        {
            if (!isUpdateWork) return;

            if (!mainCamera)
                mainCamera = Camera.main;
            if (!canvasPosition || !gameObject.activeInHierarchy || !levelSettings) return;

            float camHeight = 2f * mainCamera.orthographicSize;
            float camWidth = camHeight * mainCamera.aspect;

            detectSize = new Vector2(camWidth, camHeight) + new Vector2(size * (mainCamera.orthographicSize / 5), size * (mainCamera.orthographicSize / 5));

            if(!Application.isPlaying)
                SwithArrows();

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(canvasPosition.position, detectSize);
        }
        private void Update()
        {
            isUpdateWork = true;
            if (!mainCamera)
                mainCamera = Camera.main;
            if (!canvasPosition || !gameObject.activeInHierarchy || !levelSettings) return;

            float camHeight = 2f * mainCamera.orthographicSize;
            float camWidth = camHeight * mainCamera.aspect;

            detectSize = new Vector2(camWidth, camHeight) + new Vector2(size * (mainCamera.orthographicSize / 5), size * (mainCamera.orthographicSize / 5));

            SwithArrows();
        }
        private void SwithArrows()
        {
            foreach (var arrow in LevelSettings.instance.arrowsList)
            {

                if (IsArrowInsideCube(arrow.transform.position, canvasPosition.position, detectSize) && arrow.isWork)
                {
                    arrow.gameObject.SetActive(true);
                }
                else
                {
                    if (arrow.distanceCount > 0 && arrow.tail)
                    {
                        if (IsArrowInsideCube(arrow.tail.transform.position, canvasPosition.position, detectSize,true))
                        {
                            arrow.gameObject.SetActive(true);
                            continue;
                        }
                    }
                    arrow.gameObject.SetActive(false);
                }
            }
        }

        private bool IsArrowInsideCube(Vector3 point, Vector3 cubeCenter, Vector3 cubeSize,bool istail = false)
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