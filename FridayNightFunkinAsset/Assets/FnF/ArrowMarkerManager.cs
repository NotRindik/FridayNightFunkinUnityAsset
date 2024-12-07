using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Events;

namespace FridayNightFunkin.Editor
{
    public class ArrowMarkerManager
    {
        public List<List<ArrowMarker>> arrowMarkers = new List<List<ArrowMarker>>();

        public UnityEvent<ArrowMarker, ArrowMarkerTrackAsset> OnMarkerCountChanged = new UnityEvent<ArrowMarker, ArrowMarkerTrackAsset>();

        public UnityEvent<ArrowMarker, ArrowMarkerTrackAsset> OnArrowCountChanged = new UnityEvent<ArrowMarker, ArrowMarkerTrackAsset>();

        public UnityEvent OnListCleared = new UnityEvent();


        private static ArrowMarkerManager _instance;

        public int playerArrowCount { get; private set; }
        public int enemyArrowCount { get; private set; }

        public ArrowMarkerTrackAsset[] saveRoad = new ArrowMarkerTrackAsset[2];

        public int arrowCurrentIndex = 0;

        public static ArrowMarkerManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ArrowMarkerManager();
                }
                return _instance;

            }
            private set { }
        }

        public void AddArowMarker(ArrowMarker arrowMarker, ArrowMarkerTrackAsset road = null)
        {
            if (OnMarkerCountChanged != null)
            {
                OnMarkerCountChanged.Invoke(arrowMarker, road);
            }
        }
        private void InitializeArrowMarkers()
        {
            if (arrowMarkers.Count == 0)
            {
                for (int i = 0; i < Enum.GetValues(typeof(RoadSide)).Length; i++)
                {
                    arrowMarkers.Add(new List<ArrowMarker>());
                }
            }
        }

        public void AddMarkersToList(ArrowMarker arrowMarker, ArrowMarkerTrackAsset road = null)
        {
            InitializeArrowMarkers();
            if (!arrowMarker || !road)
                return;

            AddMarker(arrowMarker, road.roadSide, road);
            UpdateCounts();
            OnArrowCountChanged?.Invoke(arrowMarker,road);
        }
        public void AddMarker(ArrowMarker marker, RoadSide side, ArrowMarkerTrackAsset road = null)
        {
            if (!arrowMarkers[(int)side].Contains(marker))
            {
                arrowCurrentIndex++;
                marker.OnMarkerRemove += RemoveMarker;
                arrowMarkers[(int)side].Add(marker);
            }
        }

        public void RemoveMarker(ArrowMarker marker)
        {
            arrowMarkers[(int)marker.roadSide].Remove(marker);
        }

        private void UpdateCounts()
        {
            playerArrowCount = arrowMarkers[0].Count;
            enemyArrowCount = arrowMarkers[1].Count;
        }

        public void LoadDataFromRoad()
        {
            arrowMarkers.Clear();
            InitializeArrowMarkers();
            playerArrowCount = 0;
            enemyArrowCount = 0;
            OnListCleared?.Invoke();
        }


        public ArrowMarkerManager()
        {
            Initialize();
        }
        
        private void Initialize()
        {
            OnMarkerCountChanged.AddListener(AddMarkersToList);
        }

        public void IntegrityCheck(ArrowMarkerTrackAsset road = null)
        {
            if (!road)
                return;

            if (road.GetMarkerCount() != arrowMarkers[(int)road.roadSide].Count && !saveRoad[(int)road.roadSide])
            {
                for(int i = 0; i < road.GetMarkerCount(); i++)
                {
                    AddArowMarker(road.GetMarker(i) as ArrowMarker,road);
                }
                saveRoad[(int)road.roadSide] = road;
            }
        }
    }
}