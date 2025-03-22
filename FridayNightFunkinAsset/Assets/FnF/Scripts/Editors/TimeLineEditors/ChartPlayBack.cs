using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System;
using FnF.Scripts;
using FnF.Scripts.Extensions;
using FnF.Scripts.Settings;
using FridayNightFunkin.Calculations;
using FridayNightFunkin.GamePlay;
using UnityEngine.Timeline;

namespace FridayNightFunkin.Editor.TimeLineEditor
{
    [RequireComponent(typeof(PlayableDirector))]
    [ExecuteAlways]
    public class ChartPlayBack : MonoBehaviour,IService
    {
        public Arrow[] arrowsPrefab;
        private double _time;
        public LayerMask arrowsLayer;
        public PlayableDirector playableDirector;
        public LevelData levelData;
        private List<ArrowMarker> _markers = new List<ArrowMarker>();
        public ArrowTakerEnemy[] arrowTakerEnemy;
        public ArrowTakerPlayer[] arrowTakerPlayer;
        public float SpeedSave { private set; get; }

        public float SpeedCofency{ private set; get; }

        public bool playOnStart = true;

        public bool reloadChart;
        [SerializeField] private bool turnOfArrows = true;
        public static int CurrentStageIndex => PlayerPrefs.GetInt(LevelManager.STAGE_PLAYERPREFS_NAME);
        public static int CurrentDifficult => PlayerPrefs.GetInt(LevelManager.DIFFICULTY_PLAYERPREFS_NAME);

        private ArrowSwitch _arrowSwitch;
        private PlayerMissTaker _playerMissTaker;
        public RoadManager roadManager;

        public ChartContainer chartContainer;

        [Tooltip("This variable you can change only in Level Data Editor Window")]public float chartSpawnDistance;


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
                _arrowSwitch = new ArrowSwitch(this);   
            }
            _playerMissTaker = new PlayerMissTaker(this);
            roadManager = new RoadManager(playableDirector,this);
        }

        private void OnDisable()
        {
            _arrowSwitch = null;
            _playerMissTaker = null;
        }

        private void OnValidate()
        {
            if (!playableDirector)
            {
                playableDirector = GetComponent<PlayableDirector>();
            }

            if (!chartContainer)
            {
                chartContainer = FindObjectOfType<ChartContainer>();
            }
        }

        public void InitOnGameMode(SettingsManager settingsManager,LevelData levelData)
        {
            _playerMissTaker = new PlayerMissTaker(this);
            chartSpawnDistance = settingsManager.activeGameSettings.Downscroll == 1 ? levelData.stage[levelData.selectedStageIndex].chartSpawnDistance * -1 : levelData.stage[levelData.selectedStageIndex].chartSpawnDistance;
            GameStateManager.instance.OnGameStateChanged += OnGameStateChanged;
            ReloadChart();
            this.levelData = levelData;
        }
        public void ReloadChart()
        {
            if (chartContainer)
            {
                for (int i = chartContainer.transform.childCount - 1; i >= 0; i--)
                {
                    DestroyImmediate(chartContainer.transform.GetChild(i).gameObject);
                }
                chartContainer.arrowsList.Clear();
                foreach (RoadSide roadSide in Enum.GetValues(typeof(RoadSide)))
                {
                    chartContainer.arrowsList.Add(roadSide, new List<Arrow>());
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
            if (PlayerPrefs.HasKey(LevelManager.DIFFICULTY_PLAYERPREFS_NAME))
            {
                playableDirector.playableAsset = levelData.stage[levelData.selectedStageIndex].chartVariants[CurrentDifficult];
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
                _playerMissTaker.OnUpdate();
                ChangeStageOfLevel();
            }
            OnBothUpdates();
        }

        private void OnEditModeUpdate()
        {
            chartSpawnDistance = levelData.stage[levelData.selectedStageIndex].chartSpawnDistance;
            var currStage = levelData.stage[levelData.selectedStageIndex];
            if (playableDirector.playableAsset != currStage.chartVariants[levelData.selectedChartVar])
            {
                playableDirector.playableAsset = currStage.chartVariants[levelData.selectedChartVar];
                reloadChart = true;
            }
            if (reloadChart)
            {
                ReloadChart();
            }
            SaveArrowsOnSpeedChange();
            _arrowSwitch.SwitchAllArrows(turnOfArrows);
            _arrowSwitch.OnUpdate();
        }

        private void OnBothUpdates()
        {
            Counters();
            
            if (levelData != null)
            {
                MoveArrows();
            }
        }

        private void Counters()
        {
            _time = playableDirector.time;
        }

        private void MoveArrows()
        {
            foreach (KeyValuePair<RoadSide, List<Arrow>> pair in chartContainer.arrowsList)
            {
                RoadSide status = pair.Key;
                foreach (Arrow arrow in pair.Value)
                {
                    if (arrow.StartTime <= _time && arrow.EndTime + 5 + arrow.distanceCount >= _time)
                    {
                        arrow.MoveArrowByTime(_time);
                    }
                    else
                    {
                        arrow.transform.position = arrow.StartPos;
                    }
                }
            }
        }
        
        private void ChangeStageOfLevel()
        {
            if (playableDirector.duration - 1 < _time && Application.isPlaying)
            {
                playableDirector.time = 0;
                var score = G.Instance.Get<StatisticManager>().score;
                if (PlayerPrefs.GetInt(LevelManager.IS_FROM_FREE_PLAY) == 0)
                {
                    if (CurrentStageIndex == 0)
                        PlayerPrefs.SetInt($"{levelData.name}Score.temp", score);
                    else
                    {
                        PlayerPrefs.SetInt($"{levelData.name}Score.temp", PlayerPrefs.GetInt($"{levelData.name}Score.temp") + score);
                    }

                    if (levelData.stage.Length == CurrentStageIndex + 1)
                    {
                        if (PlayerPrefs.GetInt($"{levelData.name}Score.temp") > PlayerPrefs.GetInt($"{levelData.name}Score"))
                        {
                            PlayerPrefs.SetInt($"{levelData.name}Score",PlayerPrefs.GetInt($"{levelData.name}Score.temp"));
                        }
                        PlayerPrefs.SetInt(LevelManager.STAGE_PLAYERPREFS_NAME, 0);
                        PlayerPrefs.SetInt("AfterLevel", 1);
                        PlayerPrefs.SetInt($"{levelData.name}Score.temp", 0);
                        G.Instance.Get<SceneLoad>().StartLoad("MainMenu");
                    }
                    else
                    {
                        PlayerPrefs.SetInt(LevelManager.STAGE_PLAYERPREFS_NAME, CurrentStageIndex + 1);
                        G.Instance.Get<SceneLoad>().StartLoad(SceneManager.GetActiveScene().name);
                    }
                }
                else
                {
                    PlayerPrefs.SetInt(LevelManager.STAGE_PLAYERPREFS_NAME, 0);
                    
                    if(PlayerPrefs.GetInt($"{levelData.stage[levelData.selectedStageIndex].name}{ScoreManager.STAGE_PERSONAL_RECORD_PREFIX}") < score)
                        PlayerPrefs.SetInt($"{levelData.stage[levelData.selectedStageIndex].name}{ScoreManager.STAGE_PERSONAL_RECORD_PREFIX}", score);
                    PlayerPrefs.SetInt("AfterLevel", 1);
                    print($"{levelData.stage[levelData.selectedStageIndex].name}{ScoreManager.STAGE_PERSONAL_RECORD_PREFIX}");
                    
                    G.Instance.Get<SceneLoad>().StartLoad("MainMenu");
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

            arrow.transform.SetParent(chartContainer.transform);
            arrow.transform.localScale = new Vector3(1.77f, 1.77f, 1.77f);
            arrow.gameObject.name = $"Arrow[{arrowMarker.roadSide}] №:{arrowMarker.id}";
            arrow.Intialize(arrowMarker,arrowTakerTransform, this);
            chartContainer.arrowsList[roadSide].Add(arrow);
        }

        public void SaveArrows(ArrowMarker arrowMarker, ArrowMarkerTrackAsset road = null)
        {
            if (chartContainer)
            {
                SpeedSave = levelData.stage[levelData.selectedStageIndex].ChartSpeed;
                SpeedCofency = 10 / SpeedSave;
                if (!arrowMarker || !road)
                    return;
                SpawnArrow(arrowMarker, road.roadSide);
            }
        }
        private void SaveArrowsOnSpeedChange()
        {
            if (SpeedSave != levelData.stage[levelData.selectedStageIndex].ChartSpeed)
            {
                SpeedSave = levelData.stage[levelData.selectedStageIndex].ChartSpeed;
                SpeedCofency = 10 / SpeedSave;
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
        public TimelineAsset timelineAsset => director.playableAsset as TimelineAsset;
        public ChartPlayBack chartPlayBack; 
        public PlayableDirector director;
        public RoadManager(PlayableDirector director, ChartPlayBack chartPlayBack) 
        {
            this.chartPlayBack = chartPlayBack;
            this.director = director;
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