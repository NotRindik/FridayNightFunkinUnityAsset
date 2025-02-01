using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System;
using FridayNightFunkin.Calculations;
using FridayNightFunkin.GamePlay;
using FridayNightFunkin.Settings;
using AYellowpaper.SerializedCollections;
using UnityEngine.Timeline;

namespace FridayNightFunkin.Editor.TimeLineEditor
{
    [RequireComponent(typeof(PlayableDirector))]
    [ExecuteAlways]
    public class ChartPlayBack : MonoBehaviour
    {
        public Arrow[] arrows;
        private double time;
        public int arrowsLayer;
        public PlayableDirector playableDirector;
        public LevelData levelData;
        private List<ArrowMarker> markers = new List<ArrowMarker>();
        [SerializedDictionary]public SerializedDictionary<RoadSide,List<Arrow>> arrowsList = new SerializedDictionary<RoadSide,List<Arrow>>();
        public ArrowTakerEnemy[] arrowTakerEnemy;
        public ArrowTakerPlayer[] arrowTakerPlayer;
        public RectTransform chartRoad;
        public float chartSpawnDistance = 10;
        public float speedSave { private set; get; }

        public LevelDataWindow levelDataWindow { private set; get; }

        public float speedCofency{ private set; get; }

        public bool playOnStart = true;

        public bool reloadChart;
        [SerializeField] private bool TurnOfArrows = true;

        public event Action OnSpeedChanged;

        internal const string STAGE_PLAYER_PREFS_NAME = "CurrentStage";
        public int currentStageIndex => PlayerPrefs.GetInt(STAGE_PLAYER_PREFS_NAME);

        private ArrowSwitch arrowSwitch;
        private PlayerMissTaker playerMissTaker;
        public RoadManager roadManager;

        private ChartContainer _chartContainer;
        private ChartContainer chartContainer 
        { 
            get 
            {
                if(_chartContainer == null)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (transform.GetChild(i).TryGetComponent(out ChartContainer container))
                        {
                            _chartContainer = container;
                        }
                    }
                }
                return _chartContainer;

            } 
        }


        private void OnDestroy()
        {
            if (Application.isPlaying)
            {
                GameStateManager.instance.OnGameStateChanged -= OnGameStateChanged;
            }
            arrowSwitch = null;
        }

        private void OnEnable()
        {
            arrowSwitch = new ArrowSwitch(this);
            roadManager = new RoadManager(playableDirector,this);
        }

        private void OnDisable()
        {
            arrowSwitch = null;
        }

        private void OnValidate()
        {
            if (!playableDirector)
            {
                playableDirector = GetComponent<PlayableDirector>();
            }
        }

        public void SetLevelDataWindow(LevelDataWindow levelDataWindow)
        {
            this.levelDataWindow = levelDataWindow;
            levelDataWindow.OnGUIUpdate += OnChartVariantChange;
        }

        private void Start()
        {
            GameStateManager.instance.OnGameStateChanged += OnGameStateChanged;
            playerMissTaker = new PlayerMissTaker(this);
            chartSpawnDistance = ChangesByGameSettings.instance.downscroll == 1 ? chartSpawnDistance * -1 : chartSpawnDistance;
            if (playOnStart)
            {
                StartLevel();
            }
        }

        public void ReloadChart()
        {
            if (chartContainer != null)
            {
                for (int i = 0; i < chartContainer.transform.childCount; i++)
                {
                    DestroyImmediate(chartContainer.transform.GetChild(i).gameObject);
                }
                arrowsList.Clear();
                foreach (RoadSide roadSide in Enum.GetValues(typeof(RoadSide)))
                {
                    arrowsList.Add(roadSide, new List<Arrow>());
                }
                foreach (var track in roadManager.arrowMarkerTrackAssets)
                {
                    track.LoadDataFromRoad();
                }
            }
            reloadChart = false;

        }

        public void StartLevel()
        {
            if (PlayerPrefs.HasKey("Difficult"))
            {
                playableDirector.playableAsset = levelData.stage[levelDataWindow.selectedStageIndex].chartVariants[PlayerPrefs.GetInt("Difficult")];
                ReloadChart();
            }
            else
            {
                playableDirector.playableAsset = levelData.stage[levelDataWindow.selectedStageIndex].chartVariants[0];
                ReloadChart();
            }
            playableDirector.Play();
        }

        private void Update()
        {
            if (Application.isEditor)
            {
                EditorUpdate();
            }
            else
            {
                playerMissTaker.OnUpdate();
            }
            OnBothUpdates();
        }

        private void EditorUpdate()
        {
            if (reloadChart)
            {
                ReloadChart();
            }
            OnBothUpdates();
            SaveArrowsOnSpeedChange();
            arrowSwitch.SwitchAllArrows(TurnOfArrows);
        }

        private void OnBothUpdates()
        {
            Counters();

            ChangeStageOfLevel();
            if (levelData != null)
            {
                MoveArrows();
            }

            arrowSwitch.OnUpdate();
        }

        private void OnChartVariantChange()
        {
            if (levelDataWindow && levelData)
                playableDirector.playableAsset = levelData.stage[levelDataWindow.selectedStageIndex].chartVariants[levelDataWindow.selectedChartVar];
        }

        private void Counters()
        {
            time = playableDirector.time;
        }

        private void MoveArrows()
        {
            foreach (KeyValuePair<RoadSide, List<Arrow>> pair in arrowsList)
            {
                RoadSide status = pair.Key;
                foreach (Arrow arrow in pair.Value)
                {
                    if (arrow.startTime <= time && arrow.endTime + 5 + arrow.distanceCount >= time)
                    {
                        UpdateArrowEndStartPos(arrow);

                        arrow.MoveArrowByTime(time);
                    }
                    else
                    {
                        arrow.transform.position = arrow.startPos;
                    }
                }
            }
        }

        private void UpdateArrowEndStartPos(Arrow arrow)
        {
            Vector2 arrowTakerPos = arrow.roadSide == RoadSide.Player ? arrowTakerPlayer[(int)arrow.arrowSide].transform.position : arrowTakerEnemy[(int)arrow.arrowSide].transform.position;
            arrow.SetStartPos(new Vector2(arrowTakerPos.x, arrowTakerPos.y - chartSpawnDistance * (Camera.main.orthographicSize / 5)));
            arrow.SetEndPos(new Vector2(arrowTakerPos.x, arrowTakerPos.y));
        }

        private void ChangeStageOfLevel()
        {
            if (playableDirector.duration - 1 < time && Application.isPlaying)
            {
                playableDirector.time = 0;
                if (currentStageIndex == 0)
                    PlayerPrefs.SetInt($"{SceneManager.GetActiveScene().name}Score", ScoreManager.instance.score);
                else
                {
                    PlayerPrefs.SetInt($"{SceneManager.GetActiveScene().name}Score", PlayerPrefs.GetInt($"{SceneManager.GetActiveScene().name}Score") + ScoreManager.instance.score);
                }

                if (levelData.stage.Length == currentStageIndex + 1)
                {
                    PlayerPrefs.SetInt(STAGE_PLAYER_PREFS_NAME,0);
                    PlayerPrefs.SetInt("AfterLevel", 1);
                    SceneLoad.instance.StartLoad("MainMenu");
                }
                else
                {
                    PlayerPrefs.SetInt(STAGE_PLAYER_PREFS_NAME, currentStageIndex + 1);
                    SceneLoad.instance.StartLoad(SceneManager.GetActiveScene().name);
                }
            }
        }

        public void SpawnArrow(ArrowMarker arrowMarker, RoadSide roadSide)
        {
            if(arrowTakerPlayer.Length == 0 || arrowTakerEnemy.Length == 0)
            {
                Debug.Log("Hey you dont put arrow takers references on array");
                return;
            }
            if(arrows.Length == 0)
            {
                Debug.Log("bro you don't put arrows prefabs on array");
                return;
            }
            Vector2 arrowTakerPos = arrowMarker.roadSide == RoadSide.Player ? arrowTakerPlayer[(int)arrowMarker.arrowSide].transform.position : arrowTakerEnemy[(int)arrowMarker.arrowSide].transform.position;

            Vector3 arrowSpawnPos = new Vector3(arrowTakerPos.x, arrowTakerPos.y - chartSpawnDistance * (Camera.main.orthographicSize / 5), 0);
            Arrow arrow = Instantiate(arrows[(int)arrowMarker.arrowSide], arrowSpawnPos, Quaternion.identity);
            arrow.roadSide = arrowMarker.roadSide;
            arrowMarker.arrow = arrow;

            Vector2 startPos = arrowSpawnPos;

            arrow.transform.SetParent(chartContainer.transform);
            arrow.transform.localScale = new Vector3(1, 1, 1);
            arrow.gameObject.name = $"Arrow[{arrowMarker.roadSide}] №:{arrowMarker.id}";
            arrow.Intialize(arrow.GetComponent<SpriteRenderer>(), arrowMarker,startPos, arrowTakerPos, this);
            arrowsList[roadSide].Add(arrow);
        }

        public void SaveArrows(ArrowMarker arrowMarker, ArrowMarkerTrackAsset road = null)
        {
            if (levelDataWindow && chartContainer)
            {
                speedSave = levelData.stage[levelDataWindow.selectedStageIndex].chartSpeed;
                speedCofency = 10 / speedSave;
                if (!arrowMarker || !road)
                    return;
                SpawnArrow(arrowMarker, road.roadSide);
            }
        }
        private void SaveArrowsOnSpeedChange()
        {
            if (levelDataWindow)
            {
                if (speedSave != levelData.stage[levelDataWindow.selectedStageIndex].chartSpeed)
                {
                    speedSave = levelData.stage[levelDataWindow.selectedStageIndex].chartSpeed;
                    speedCofency = 10 / speedSave;
                }
            }
        }
        private void OnGameStateChanged(GameState currenState)
        {
            if (currenState == GameState.Paused)
            {
                playableDirector.Pause();
            }
            else
            {
                playableDirector.Resume();
            }
        }
    }

    [System.Serializable]
    public class RoadManager 
    {
        public ArrowMarkerTrackAsset[] arrowMarkerTrackAssets => GetRoads();
        public TimelineAsset timelineAsset;
        public ChartPlayBack chartPlayBack;
        public RoadManager(PlayableDirector director, ChartPlayBack chartPlayBack) 
        {
            timelineAsset = director.playableAsset as TimelineAsset;
            this.chartPlayBack = chartPlayBack;
        }   

        private ArrowMarkerTrackAsset[] GetRoads()
        {
            List<ArrowMarkerTrackAsset> trackAssets = new List<ArrowMarkerTrackAsset>();
            if (timelineAsset != null)
            {
                int trackCount = timelineAsset.outputTrackCount;
                for (int i = 0; i < trackCount; i++)
                {
                    TrackAsset track = timelineAsset.GetOutputTrack(i);
                    if (track is ArrowMarkerTrackAsset)
                    {
                        ArrowMarkerTrackAsset trackAsset = track as ArrowMarkerTrackAsset;
                        trackAssets.Add(trackAsset);
                        trackAsset.chartPlayBack = chartPlayBack;
                    }
                }
            }
            return trackAssets.ToArray();
        }
    }
}