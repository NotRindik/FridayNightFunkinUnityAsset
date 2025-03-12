using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using FnF.Scripts.Settings;
using FridayNightFunkin.Calculations;
using FridayNightFunkin.GamePlay;
using FridayNightFunkin.Settings;
using UnityEngine.Timeline;

namespace FridayNightFunkin.Editor.TimeLineEditor
{
    [RequireComponent(typeof(PlayableDirector))]
    [ExecuteAlways]
    public class ChartPlayBack : MonoBehaviour
    {
        public Arrow[] arrowsPrefab;
        private double time;
        public LayerMask arrowsLayer;
        public PlayableDirector playableDirector;
        public LevelData levelData;
        private List<ArrowMarker> _markers = new List<ArrowMarker>();
        public ArrowTakerEnemy[] arrowTakerEnemy;
        public ArrowTakerPlayer[] arrowTakerPlayer;
        public RectTransform chartRoad;
        public float chartSpawnDistance = 10;
        public float speedSave { private set; get; }

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

        public ChartContainer ChartContainer;


        private void OnDestroy()
        {
            if (Application.isPlaying)
            {
                GameStateManager.instance.OnGameStateChanged -= OnGameStateChanged;
            }
        }

        private void OnEnable()
        {
            if (!Application.isPlaying)
            {
                arrowSwitch = new ArrowSwitch(this);   
            }
            playerMissTaker = new PlayerMissTaker(this);
            roadManager = new RoadManager(playableDirector,this);
        }

        private void OnDisable()
        {
            arrowSwitch = null;
            playerMissTaker = null;
        }

        private void OnValidate()
        {
            if (!playableDirector)
            {
                playableDirector = GetComponent<PlayableDirector>();
            }
        }

        private void Start()
        {
            GameStateManager.instance.OnGameStateChanged += OnGameStateChanged;
            playerMissTaker = new PlayerMissTaker(this);
            chartSpawnDistance = SettingsManager.Instance.activeGameSettings.Downscroll == 1 ? chartSpawnDistance * -1 : chartSpawnDistance;
            if (playOnStart && Application.isPlaying)
            {
                StartLevel();
            }
        }

        public void ReloadChart()
        {
            if (ChartContainer)
            {
                for (int i = 0; i < ChartContainer.transform.childCount; i++)
                {
                    DestroyImmediate(ChartContainer.transform.GetChild(i).gameObject);
                }
                ChartContainer.arrowsList.Clear();
                foreach (RoadSide roadSide in Enum.GetValues(typeof(RoadSide)))
                {
                    ChartContainer.arrowsList.Add(roadSide, new List<Arrow>());
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
                playableDirector.playableAsset = levelData.stage[levelData.selectedStageIndex].chartVariants[PlayerPrefs.GetInt("Difficult")];
                ReloadChart();
            }
            else
            {
                playableDirector.playableAsset = levelData.stage[levelData.selectedStageIndex].chartVariants[0];
                ReloadChart();
            }
            playableDirector.Play();
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                OnEditModeUpdate();
            }
            else
            {
                playerMissTaker.OnUpdate();
                ChangeStageOfLevel();
            }
            OnBothUpdates();
        }

        private void OnEditModeUpdate()
        {
            if (reloadChart)
            {
                ReloadChart();
            }
            SaveArrowsOnSpeedChange();
            arrowSwitch.SwitchAllArrows(TurnOfArrows);
            arrowSwitch.OnUpdate();
        }

        private void OnBothUpdates()
        {
            Counters();
            
            if (levelData != null)
            {
                MoveArrows();
            }
        }

        private void OnChartVariantChange()
        {
            if (levelData)
                playableDirector.playableAsset = levelData.stage[levelData.selectedStageIndex].chartVariants[levelData.selectedChartVar];
        }

        private void Counters()
        {
            time = playableDirector.time;
        }

        private void MoveArrows()
        {
            foreach (KeyValuePair<RoadSide, List<Arrow>> pair in ChartContainer.arrowsList)
            {
                RoadSide status = pair.Key;
                foreach (Arrow arrow in pair.Value)
                {
                    if (arrow.startTime <= time && arrow.endTime + 5 + arrow.distanceCount >= time)
                    {
                        arrow.MoveArrowByTime(time);
                    }
                    else
                    {
                        arrow.transform.position = arrow.startPos;
                    }
                }
            }
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
            if(arrowsPrefab.Length == 0)
            {
                Debug.Log("bro you don't put arrows prefabs on array");
                return;
            }
            Transform arrowTakerTransform = arrowMarker.roadSide == RoadSide.Player ? arrowTakerPlayer[(int)arrowMarker.arrowSide].transform : arrowTakerEnemy[(int)arrowMarker.arrowSide].transform;

            Vector3 arrowSpawnPos = new Vector3(arrowTakerTransform.position.x, arrowTakerTransform.position.y - chartSpawnDistance * (Camera.main.orthographicSize / 5), 0);
            Arrow arrow = Instantiate(arrowsPrefab[(int)arrowMarker.arrowSide], arrowSpawnPos, Quaternion.identity);
            arrow.roadSide = arrowMarker.roadSide;
            arrowMarker.arrow = arrow;
            

            arrow.transform.SetParent(ChartContainer.transform);
            arrow.transform.localScale = new Vector3(1.77f, 1.77f, 1.77f);
            arrow.gameObject.name = $"Arrow[{arrowMarker.roadSide}] №:{arrowMarker.id}";
            arrow.Intialize(arrowMarker,arrowTakerTransform,chartSpawnDistance, this);
            ChartContainer.arrowsList[roadSide].Add(arrow);
        }

        public void SaveArrows(ArrowMarker arrowMarker, ArrowMarkerTrackAsset road = null)
        {
            if (ChartContainer)
            {
                speedSave = levelData.stage[levelData.selectedStageIndex].chartSpeed;
                speedCofency = 10 / speedSave;
                if (!arrowMarker || !road)
                    return;
                SpawnArrow(arrowMarker, road.roadSide);
            }
        }
        private void SaveArrowsOnSpeedChange()
        {
            if (speedSave != levelData.stage[levelData.selectedStageIndex].chartSpeed)
            {
                speedSave = levelData.stage[levelData.selectedStageIndex].chartSpeed;
                speedCofency = 10 / speedSave;
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