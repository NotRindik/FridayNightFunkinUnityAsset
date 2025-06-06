using System;
using FridayNightFunkin.GamePlay;
using UnityEditor;
using UnityEngine;

namespace FridayNightFunkin.Editor.TimeLineEditor
{
    [ExecuteInEditMode]
    public class ArrowsSpawner : MonoBehaviour,IObserver<(ArrowMarker,ArrowMarkerTrackAsset)>
    {
        public Arrow[] arrowsPrefab;
        public ChartPlayBack chartPlayBack;

        
        private void Update()
        {
            foreach (var trackAsset in chartPlayBack.roadManager.arrowMarkerTrackAssets)
            {
                trackAsset.onAddArrow.AddListener(this);
            }
        }

        public void SpawnArrow(ArrowMarker arrowMarker, RoadSide roadSide)
        {
            if (chartPlayBack.arrowTakerPlayer.Length == 0 || chartPlayBack.arrowTakerEnemy.Length == 0)
            {
                Debug.Log("Hey you dont put arrow takers references on array");
                return;
            }
            if (arrowsPrefab.Length == 0)
            {
                Debug.Log("bro you don't put arrows prefabs on array");
                return;
            }
            Transform arrowTakerTransform = arrowMarker.roadSide == RoadSide.Player ? chartPlayBack.arrowTakerPlayer[(int)arrowMarker.arrowSide].transform : chartPlayBack.arrowTakerEnemy[(int)arrowMarker.arrowSide].transform;

            Vector3 arrowSpawnPos = new Vector3(arrowTakerTransform.position.x, arrowTakerTransform.position.y - chartPlayBack.chartSpawnDistance * (Camera.main.orthographicSize / 5), 0);
            Arrow arrow = Instantiate(arrowsPrefab[(int)arrowMarker.arrowSide], arrowSpawnPos, Quaternion.identity);
            arrow.roadSide = arrowMarker.roadSide;

            arrow.transform.SetParent(chartPlayBack.chartContainer.transform);
            arrow.transform.localScale = new Vector3(1.77f, 1.77f, 1.77f);
            arrow.gameObject.name = $"Arrow[{arrowMarker.roadSide}] №:{arrowMarker.id}";
            arrow.Intialize(arrowMarker, arrowTakerTransform, chartPlayBack);
            chartPlayBack.chartContainer.arrowsList[roadSide].Add(arrow);
        }

        public void SaveArrows(ArrowMarker arrowMarker, ArrowMarkerTrackAsset road = null)
        {
            if (chartPlayBack.chartContainer)
            {
                if (!arrowMarker || !road)
                    return;
                SpawnArrow(arrowMarker, road.roadSide);
            }
        }
        public void OnInvoke((ArrowMarker, ArrowMarkerTrackAsset) data)
        {
            SaveArrows(data.Item1,data.Item2);
        }
    }
}