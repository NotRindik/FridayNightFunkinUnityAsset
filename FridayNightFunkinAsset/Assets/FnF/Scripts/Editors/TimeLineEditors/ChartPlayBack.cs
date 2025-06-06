using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.InteropServices;
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
    public unsafe class ChartPlayBack : MonoBehaviour, IService
    {
        public double Time
        {
            get
            {
                if (!playableDirector)
                {
                    throw new NullReferenceException("PlayableDirector is not set");
                }
                return playableDirector.time;
            }
            private set{}
        } 

        public LayerMask arrowsLayer;
        public PlayableDirector playableDirector;
        public LevelData levelData;

        public bool playOnStart = true;

        public bool reloadChart;
        [SerializeField] public bool turnOfArrows = true;
        public static int CurrentStageIndex => PlayerPrefs.GetInt(LevelSaveConst.STAGE_PLAYERPREFS_NAME);
        public static int CurrentDifficult => PlayerPrefs.GetInt(LevelSaveConst.DIFFICULTY_PLAYERPREFS_NAME);

        private ArrowSwitch _arrowSwitch;
        private PlayerMissTaker _playerMissTaker;
        public RoadManager roadManager;
        public ChartContainer chartContainer;

        public ArrowTakerEnemy[] arrowTakerEnemy;
        public ArrowTakerPlayer[] arrowTakerPlayer;
        
        [Tooltip("This variable you can change only in Level Data Editor Window")]
        public float chartSpawnDistance;
        
        private void Start()
        {
            if (Application.isEditor)
            {
                reloadChart = true;
            }
        }

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
            roadManager = new RoadManager(playableDirector);
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
        }

        public void InitOnGameMode(SettingsManager settingsManager)
        {
            _playerMissTaker = new PlayerMissTaker(this);
            chartSpawnDistance = settingsManager.activeGameSettings.Downscroll == 1
                ? levelData.stage[levelData.selectedStageIndex].chartSpawnDistance * -1
                : levelData.stage[levelData.selectedStageIndex].chartSpawnDistance;
            GameStateManager.instance.OnGameStateChanged += OnGameStateChanged;
            ReloadChart();
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
            if (PlayerPrefs.HasKey(LevelSaveConst.DIFFICULTY_PLAYERPREFS_NAME))
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
            if (levelData.stage[levelData.selectedStageIndex].chartVariants.Length > 0)
            {
                if (playableDirector.playableAsset != currStage.chartVariants[levelData.selectedChartVar])
                {
                    playableDirector.playableAsset = currStage.chartVariants[levelData.selectedChartVar];
                    reloadChart = true;
                }
                if (reloadChart)
                {
                    ReloadChart();
                }
                _arrowSwitch.SwitchAllArrows(turnOfArrows);
                _arrowSwitch.OnUpdate();   
            }
        }

        private void OnBothUpdates()
        {
        }
        
        private void ChangeStageOfLevel()
        {
            if (playableDirector.duration - 1 < Time && Application.isPlaying)
            {
                playableDirector.time = 0;
                var score = G.Instance.Get<StatisticManager>().score;
                if (PlayerPrefs.GetInt(LevelSaveConst.IS_FROM_FREE_PLAY) == 0)
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
                        PlayerPrefs.SetInt(LevelSaveConst.STAGE_PLAYERPREFS_NAME, 0);
                        PlayerPrefs.SetInt("AfterLevel", 1);
                        PlayerPrefs.SetInt($"{levelData.name}Score.temp", 0);
                        G.Instance.Get<SceneLoad>().StartLoad("MainMenu");
                    }
                    else
                    {
                        PlayerPrefs.SetInt(LevelSaveConst.STAGE_PLAYERPREFS_NAME, CurrentStageIndex + 1);
                        G.Instance.Get<SceneLoad>().StartLoad(SceneManager.GetActiveScene().name);
                    }
                }
                else
                {
                    PlayerPrefs.SetInt(LevelSaveConst.STAGE_PLAYERPREFS_NAME, 0);
                    
                    if(PlayerPrefs.GetInt($"{levelData.stage[levelData.selectedStageIndex].name}{ScoreManager.STAGE_PERSONAL_RECORD_PREFIX}") < score)
                        PlayerPrefs.SetInt($"{levelData.stage[levelData.selectedStageIndex].name}{ScoreManager.STAGE_PERSONAL_RECORD_PREFIX}", score);
                    PlayerPrefs.SetInt("AfterLevel", 1);
                    print($"{levelData.stage[levelData.selectedStageIndex].name}{ScoreManager.STAGE_PERSONAL_RECORD_PREFIX}");
                    
                    G.Instance.Get<SceneLoad>().StartLoad("MainMenu");
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
        public TimelineAsset timelineAsset => director.playableAsset as TimelineAsset;
        public PlayableDirector director;
        public RoadManager(PlayableDirector director) 
        {
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
                    }
                }
            }
            return trackAssets.ToArray();
        }
    }
}

