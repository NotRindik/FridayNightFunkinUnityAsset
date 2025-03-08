using FridayNightFunkin.Editor.TimeLineEditor;
using System;
using UnityEngine;

namespace FridayNightFunkin.Editor
{
    public class ArrowSwitch
    {
        protected Vector2 detectSize;
        public float size;
        protected Transform canvasPosition => mainCamera.transform;

        protected Camera _mainCamera;
        protected Camera mainCamera 
        {
            get
            {
                if(_mainCamera == null)
                {
                    _mainCamera = Camera.main;
                }
                return _mainCamera;
            }
            set { }
        }

        protected ChartPlayBack ChartPlayback;

        protected bool lastSwitch;

        public ArrowSwitch(ChartPlayBack chartPlayback)
        {
            this.ChartPlayback = chartPlayback;
        }
        public virtual void OnUpdate()
        {
            if (!canvasPosition) return;

            float camHeight = 2f * mainCamera.orthographicSize;
            float camWidth = camHeight * mainCamera.aspect;

            detectSize = new Vector2(camWidth, camHeight) + new Vector2(size * (mainCamera.orthographicSize / 5), size * (mainCamera.orthographicSize / 5));

            SwithArrows();
        }

        public void SwitchAllArrows(bool isOn)
        {
            if (isOn != lastSwitch)
            {
                lastSwitch = isOn;
                foreach (RoadSide item in Enum.GetValues(typeof(RoadSide)))
                {
                    if (ChartPlayback.ChartContainer.arrowsList.ContainsKey(item))
                    {
                        foreach (var arrow in ChartPlayback.ChartContainer.arrowsList[item])
                        {
                            if (arrow == null)
                            {
                                return;
                            }

                            if (arrow.gameObject.activeInHierarchy != isOn)
                                arrow.gameObject.SetActive(isOn);
                        }
                    }
                }
            }
        }
        private void SwithArrows()
        {
            if (ChartPlayback.ChartContainer.arrowsList.Count == Enum.GetValues(typeof(RoadSide)).Length)
            {
                foreach (RoadSide item in Enum.GetValues(typeof(RoadSide)))
                {
                    foreach (var arrow in ChartPlayback.ChartContainer.arrowsList[item])
                    {
                        if (arrow == null)
                        {
                            return;
                        }

                        if (IsArrowInsideCube(arrow.transform.position, canvasPosition.position, detectSize) && arrow.isWork)
                        {
                            arrow.gameObject.SetActive(true);
                        }
                        else
                        {
                            if (arrow.distanceCount > 0 && arrow.tail)
                            {
                                if (IsArrowInsideCube(arrow.tail.transform.position, canvasPosition.position, detectSize, true))
                                {
                                    arrow.gameObject.SetActive(true);
                                    continue;
                                }
                            }
                            arrow.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        protected bool IsArrowInsideCube(Vector3 point, Vector3 cubeCenter, Vector3 cubeSize,bool istail = false)
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