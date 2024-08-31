using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

namespace FridayNightFunkin.Editor.TimeLineEditor
{
    [ExecuteAlways]
    public class СhartContainer : MonoBehaviour
    {
        public Arrow[] arrows;
        public double time;
        public int arrowsLayer;
        public PlayableDirector playableDirector;
        private List<ArrowMarker> markers = new List<ArrowMarker>();
        public RectTransform chartRoad;
        public float chartSpawnDistance = 10;

        private bool isSaveCharts = false;
        private int[] markerCount = new int[] { 0, 0 };
        private float speedSave;

        private List<uint> distanceCount = new List<uint>();
        private List<double> arrowsTime = new List<double>();   
        private List<TrackAsset> signalTracks;

        private bool isMarkerCountChange;
        private LevelSettings levelSettings;
        private EditModeArrowTaker editModeArrow => EditModeArrowTaker.instance;

        private float speedCofency;

        
        private void Start()
        {
            if (Application.isPlaying)
            {
                GameStateManager.instance.OnGameStateChanged += OnGameStateChanged;
            }   
        }

        private void Update()
        {
            time = playableDirector.time;

            if (levelSettings != null)
            {

                for (int i = 0; i < levelSettings.arrowsList.Count; i++)
                {
                    var arrow = levelSettings.arrowsList[i];
                    if (arrow.startTime <= time && arrow.endTime + 5 + arrow.distanceCount >= time)
                    {
                        if (arrow.characterSide == CharacterSide.Player)
                        {
                            arrow.SetStartPos(new Vector2(levelSettings.arrowsPlayer[(int)arrow.arrowSide].transform.position.x, levelSettings.arrowsPlayer[(int)arrow.arrowSide].transform.position.y - chartSpawnDistance * (Camera.main.orthographicSize / 5)));
                            arrow.SetEndPos(new Vector2(levelSettings.arrowsPlayer[(int)arrow.arrowSide].transform.position.x, levelSettings.arrowsPlayer[(int)arrow.arrowSide].transform.position.y));
                        }
                        else
                        {
                            arrow.SetStartPos(new Vector2(levelSettings.arrowsEnemy[(int)arrow.arrowSide].transform.position.x, levelSettings.arrowsEnemy[(int)arrow.arrowSide].transform.position.y - chartSpawnDistance * (Camera.main.orthographicSize / 5)));
                            arrow.SetEndPos(new Vector2(levelSettings.arrowsEnemy[(int)arrow.arrowSide].transform.position.x, levelSettings.arrowsEnemy[(int)arrow.arrowSide].transform.position.y));
                        }

                        arrow.transform.position = new ArrowArchitect(arrow.arrowSide, time).CalculateArrowPos(arrow.startPos, arrow.endPos, arrow.startTime, arrow.endTime);
                    }
                    else
                    {
                        arrow.transform.position = arrow.startPos;
                    }
                }

                SaveChartFromTimeLine(playableDirector, "BF", markers);
                if (!EditorApplication.isPlayingOrWillChangePlaymode)
                {
                    if (isSaveCharts)
                    {
                        for (int i = 0; i < transform.childCount;)
                        {
                            DestroyImmediate(transform.GetChild(i).gameObject);
                        }
                        levelSettings.arrowsList = new List<Arrow>();
                        SpawnChart();
                        isSaveCharts = false;
                    }
                }
            }
            else
            {
                levelSettings = LevelSettings.instance;
            }
        }
        public void SpawnArrow(ArrowSide arrowSide, double time, uint distanceCount, int road = 0)
        {
            Vector3 arrowSpawnPos = Vector2.zero;
            Arrow arrow;

            if (road == 0)
            {
                arrowSpawnPos = new Vector3(levelSettings.arrowsPlayerPos[(int)arrowSide].x, levelSettings.arrowsPlayerPos[(int)arrowSide].y - chartSpawnDistance * (Camera.main.orthographicSize / 5), levelSettings.arrowsPlayerPos[(int)arrowSide].z);
                arrow = Instantiate(arrows[(int)arrowSide], arrowSpawnPos, Quaternion.identity);
                arrow.characterSide = CharacterSide.Player;
            }
            else
            {
                arrowSpawnPos = new Vector3(levelSettings.arrowsEnemyPos[(int)arrowSide].x, levelSettings.arrowsEnemyPos[(int)arrowSide].y - chartSpawnDistance * (Camera.main.orthographicSize / 5), levelSettings.arrowsEnemyPos[(int)arrowSide].z);
                arrow = Instantiate(arrows[(int)arrowSide], arrowSpawnPos, Quaternion.identity);
                arrow.characterSide = CharacterSide.Enemy;

            }

            Vector2 startPos = arrowSpawnPos;
            Vector2 endPos = new Vector2(arrowSpawnPos.x, arrowSpawnPos.y + 10 + (Camera.main.orthographicSize / 5));

            arrow.transform.SetParent(transform);
            arrow.Intialize(arrow.GetComponent<SpriteRenderer>(), distanceCount, startPos, endPos, time - speedCofency, time, markers.Count);
            arrow.transform.localScale = new Vector3(150, 150, 150);
            arrow.gameObject.name = "Arrow №:" + markers.Count.ToString();
            levelSettings.arrowsList.Add(arrow);
        }

        public void SpawnChart()
        {
            int[] markerCounts = new int[] { markerCount[0], markerCount[1] };
            for (int j = 0; j < markerCounts.Length; j++)
            {
                for (int i = 0; i < markerCounts[j];)
                {
                    markerCounts[j]--;

                    if (markers[i].GetType().IsAssignableFrom(typeof(PurpleLeftArrowMarker)))
                    {
                        SpawnArrow(ArrowSide.LeftArrow, markers[i].time, markers[i].distanceCount, j);
                        markers.RemoveAt(i);
                    }
                    else if (markers[i].GetType().IsAssignableFrom(typeof(BlueDownArrowMarker)))
                    {
                        SpawnArrow(ArrowSide.DownArrow, markers[i].time, markers[i].distanceCount, j);
                        markers.RemoveAt(i);
                    }
                    else if (markers[i].GetType().IsAssignableFrom(typeof(GreenUpArrowMarker)))
                    {
                        SpawnArrow(ArrowSide.UpArrow, markers[i].time, markers[i].distanceCount, j);
                        markers.RemoveAt(i);
                    }
                    else if (markers[i].GetType().IsAssignableFrom(typeof(RedRightArrowMarker)))
                    {
                        SpawnArrow(ArrowSide.RightArrow, markers[i].time, markers[i].distanceCount, j);
                        markers.RemoveAt(i);
                    }
                }

            }
        }

        void SaveChartFromTimeLine(PlayableDirector director, string streamName, List<ArrowMarker> marker)
        {
            signalTracks = new List<TrackAsset>();
            GetSignalTracks(director, "BF", marker);
            GetSignalTracks(director, "Enemy", marker);
            foreach (var signalAsset in director.playableAsset.outputs)
            {
                if (signalAsset.streamName == streamName)
                {
                    if (IsMarkersCountValue(signalTracks))
                    {
                        for (int i = 0; i < signalTracks.Count; i++)
                        {
                            isMarkerCountChange = true;
                            markerCount[i] = signalTracks[i].GetMarkerCount();
                        }
                    }
                    IsNeedSave(signalTracks, marker, IsTimeChanged(signalTracks, arrowsTime), IsHoldDistanceChanges(signalTracks, distanceCount));
                }
            }
        }

        public void GetSignalTracks(PlayableDirector director, string streamName, List<ArrowMarker> marker)
        {
            int k = 0;
            foreach (var signalAsset in director.playableAsset.outputs)
            {
                if (signalAsset.streamName == streamName)
                {
                    k++;
                    signalTracks.Add(signalAsset.sourceObject as TrackAsset);
                }
            }
        }

        private void IsNeedSave(List<TrackAsset> signalTrack, List<ArrowMarker> arrowMarkers, bool isTimeChanged, bool isDistanceChanged)
        {
            if (speedSave != levelSettings.chartSpeed || isTimeChanged || isDistanceChanged || isMarkerCountChange)
            {
                speedSave = levelSettings.chartSpeed;
                speedCofency = 10 / speedSave;
                isMarkerCountChange = false;
                isSaveCharts = true;
                for (int j = 0; j < signalTrack.Count; j++)
                {
                    for (int i = 0; i < signalTrack[j].GetMarkerCount(); i++)
                    {
                        arrowMarkers.Add(signalTrack[j].GetMarker(i) as ArrowMarker);
                    }
                }
            }
        }

        private bool IsMarkersCountValue(List<TrackAsset> signalTrack)
        {
            for (int i = 0; i < markerCount.Length; i++)
            {
                var getMarkerCount = signalTrack[i].GetMarkerCount();
                if (markerCount[i] != getMarkerCount) return true;
            }
            return false;
        }

        private bool IsHoldDistanceChanges(List<TrackAsset> signalTrack, List<uint> distancesCount)
        {
            var markerCounts = markerCount[0] + markerCount[1];
            bool readyToReturn = false;

            if (distancesCount.Count < markerCounts)
            {
                int baseCount = distancesCount.Count;
                for (int i = 0; i < markerCounts - baseCount; i++)
                {
                    distancesCount.Add(0);
                }
            }
            if (distancesCount.Count > markerCounts)
            {
                int baseCount = distancesCount.Count;
                for (int i = 0; i < baseCount - markerCounts; i++)
                {
                    distancesCount.RemoveAt(distancesCount.Count - 1);
                }
            }
            for (int j = 0; j < markerCounts; j++)
            {
                var distance = GetDistanceFromSignalTracks(signalTrack);
                if (distancesCount[j] != distance[j])
                {
                    distancesCount[j] = distance[j];
                    readyToReturn = true;
                }

            }
            if (readyToReturn)
            {
                return true;
            }
            return false;
        }
        public List<uint> GetDistanceFromSignalTracks(List<TrackAsset> signalTrack)
        {
            List<uint> distance = new List<uint>();
            for (int i = 0; i < signalTrack.Count; i++)
            {
                for (int f = 0; f < signalTrack[i].GetMarkerCount(); f++)
                {
                    distance.Add(((ArrowMarker)signalTrack[i].GetMarker(f)).distanceCount);
                }
            }
            return distance;
        }

        public List<double> GetTimeFromSignalTracks(List<TrackAsset> signalTrack)
        {
            List<double> time = new List<double>();
            for (int i = 0; i < signalTrack.Count; i++)
            {
                for (int f = 0; f < signalTrack[i].GetMarkerCount(); f++)
                {
                    time.Add(signalTrack[i].GetMarker(f).time);
                }
            }

            return time;
        }
        private bool IsTimeChanged(List<TrackAsset> signalTrack, List<double> arrowTimes)
        {
            bool readyToReturn = false;
            var markerCounts = markerCount[0] + markerCount[1];
            if (arrowTimes.Count < markerCounts)
            {
                int baseCount = arrowTimes.Count;
                for (int i = 0; i < markerCounts - baseCount; i++)
                {
                    arrowTimes.Add(0);
                }
            }
            if (arrowTimes.Count > markerCounts)
            {
                int baseCount = arrowTimes.Count;
                for (int i = 0; i < baseCount - markerCounts; i++)
                {
                    arrowTimes.RemoveAt(arrowTimes.Count - 1);
                }
            }
            for (int j = 0; j < markerCounts; j++)
            {
                var markerTime = GetTimeFromSignalTracks(signalTrack);
                if (arrowTimes[j] != markerTime[j])
                {
                    arrowTimes[j] = markerTime[j];
                    readyToReturn = true;
                }

            }
            if (readyToReturn)
            {
                return true;
            }
            return false;
        }

        private void OnGameStateChanged(GameState currenState)
        {
            if (currenState == GameState.Paused) 
                playableDirector.Pause(); 
            else 
                playableDirector.Resume();
        }

        private void OnDestroy()
        {
            if (Application.isPlaying)
            {
                GameStateManager.instance.OnGameStateChanged -= OnGameStateChanged;
            }
        }
    }
}