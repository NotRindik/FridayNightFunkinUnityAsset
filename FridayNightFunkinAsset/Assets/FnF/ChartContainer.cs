using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;

namespace FridayNightFunkin.Editor.TimeLineEditor
{
    [ExecuteAlways]
    public class ChartContainer : MonoBehaviour
    {
        public Arrow[] arrows;
        private double time;
        public int arrowsLayer;
        public PlayableDirector playableDirector;
        private List<ArrowMarker> markers = new List<ArrowMarker>();
        public RectTransform chartRoad;
        public float chartSpawnDistance = 10;
        public float speedSave { private set; get; }

        private LevelSettings levelSettings;

        public static ChartContainer instance;

        public float speedCofency{ private set; get; }

        public bool playOnStart = true;

        public bool reloadChart;

        public int playerArrowCount;
        public int enemyArrowCount;

        public UnityEvent OnSpeedChanged = new UnityEvent();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else 
            {
                DestroyImmediate(this.gameObject);
            }
        }

        private void Start()
        {
            TryGetComponent(out PlayableDirector playableDirector);
            this.playableDirector = playableDirector;
            levelSettings = LevelSettings.instance;
            if (Application.isPlaying && levelSettings)
            {
                GameStateManager.instance.OnGameStateChanged += OnGameStateChanged;
                chartSpawnDistance = ServiceLocator.instance.Get<ChangesByGameSettings>().downscroll == 1 ? chartSpawnDistance * -1 : chartSpawnDistance;
                if (playOnStart)
                {
                    StartLevel();
                }
            }
            InitDelegates();
        }

        public void ReloadChart()
        {
            for (int i = 0; i < transform.childCount;)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
            LevelSettings.instance.arrowsList.Clear();
            ArrowMarkerManager.instance.LoadDataFromRoad();
            InitDelegates();
            reloadChart = false;
        }

        public void StartLevel()
        {
            ArrowMarkerManager.instance.OnArrowCountChanged.AddListener(SaveArrows);
            if (PlayerPrefs.HasKey("Difficult"))
            {
                playableDirector.playableAsset = levelSettings.stage[levelSettings.stageIndex].chartVariants[PlayerPrefs.GetInt("Difficult")];
                ReloadChart();
            }
            else
            {
                playableDirector.playableAsset = levelSettings.stage[levelSettings.stageIndex].chartVariants[0];
            }

            for (int i = 0; i < levelSettings.arrowsList.Count; i++)
            {
                var arrow = levelSettings.arrowsList[i];
                UpdateArrowEndStartPos(arrow);

                arrow.transform.position = arrow.startPos;
            }
            playableDirector.Play();
        }

        private void Update()
        {
            if (instance == null)
            {
                instance = this;
            }
            Counters();

            if (reloadChart)
            {
                ReloadChart();
            }

            ChangeStageOfLevel();

            if (levelSettings != null)
            {
                MoveArrows();

            }
            else
            {
                levelSettings = LevelSettings.instance;
            }
        }

        private void Counters()
        {
            time = playableDirector.time;
            playerArrowCount = ArrowMarkerManager.instance.playerArrowCount;
            enemyArrowCount = ArrowMarkerManager.instance.enemyArrowCount;
        }

        private void MoveArrows()
        {
            for (int i = 0; i < levelSettings.arrowsList.Count; i++)
            {
                var arrow = levelSettings.arrowsList[i];
                if (arrow == null)
                {
                    ReloadChart();
                }
                if (arrow.startTime <= time && arrow.endTime + 5 + arrow.distanceCount >= time)
                {
                    UpdateArrowEndStartPos(arrow);

                    arrow.transform.position = new ArrowArchitect(time).CalculateArrowPos(arrow.startPos, arrow.endPos, arrow.startTime, arrow.endTime);
                }
                else
                {
                    arrow.transform.position = arrow.startPos;
                }
            }
        }

        private void UpdateArrowEndStartPos(Arrow arrow)
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
        }

        private void ChangeStageOfLevel()
        {
            if (playableDirector.duration - 1 < time && Application.isPlaying)
            {
                playableDirector.time = 0;
                if (LevelSettings.instance.stageIndex == 0)
                    PlayerPrefs.SetInt($"{SceneManager.GetActiveScene().name}Score", ScoreManager.instance.score);
                else
                {
                    PlayerPrefs.SetInt($"{SceneManager.GetActiveScene().name}Score", PlayerPrefs.GetInt($"{SceneManager.GetActiveScene().name}Score") + ScoreManager.instance.score);
                }

                if (LevelSettings.instance.stage.Length == LevelSettings.instance.stageIndex + 1)
                {
                    LevelSettings.instance.SetStage(0);
                    PlayerPrefs.SetInt("AfterLevel", 1);
                    SceneLoad.instance.StartLoad("MainMenu");
                }
                else
                {
                    var nextStage = LevelSettings.instance.stageIndex + 1;
                    LevelSettings.instance.SetStage(nextStage);
                    SceneLoad.instance.StartLoad(SceneManager.GetActiveScene().name);
                }
            }
        }

        private void InitDelegates()
        {
            ArrowMarkerManager.instance.OnArrowCountChanged.AddListener(SaveArrows);
            levelSettings.OnSpeedChanges.AddListener(SaveArrowsOnSpeedChange);
        }

        public void SpawnArrow(ArrowMarker arrowMarker, RoadSide roadSide)
        {
            Vector3 arrowSpawnPos = Vector2.zero;
            Arrow arrow;
            if (levelSettings.arrowsPlayerPos.Count == 0) return;

            if (roadSide == RoadSide.Player)
            {
                arrowSpawnPos = new Vector3(levelSettings.arrowsPlayerPos[(int)arrowMarker.arrowSide].x, levelSettings.arrowsPlayerPos[(int)arrowMarker.arrowSide].y - chartSpawnDistance * (Camera.main.orthographicSize / 5), levelSettings.arrowsPlayerPos[(int)arrowMarker.arrowSide].z);
                arrow = Instantiate(arrows[(int)arrowMarker.arrowSide], arrowSpawnPos, Quaternion.identity);
                arrow.characterSide = CharacterSide.Player;
            }
            else
            {
                arrowSpawnPos = new Vector3(levelSettings.arrowsEnemyPos[(int)arrowMarker.arrowSide].x, levelSettings.arrowsEnemyPos[(int)arrowMarker.arrowSide].y - chartSpawnDistance * (Camera.main.orthographicSize / 5), levelSettings.arrowsEnemyPos[(int)arrowMarker.arrowSide].z);
                arrow = Instantiate(arrows[(int)arrowMarker.arrowSide], arrowSpawnPos, Quaternion.identity);
                arrow.characterSide = CharacterSide.Enemy;

            }

            Vector2 startPos = arrowSpawnPos;
            Vector2 endPos = new Vector2(arrowSpawnPos.x, arrowSpawnPos.y + 10 + (Camera.main.orthographicSize / 5));

            arrow.transform.SetParent(transform);
            arrow.Intialize(arrow.GetComponent<SpriteRenderer>(), arrowMarker,startPos, endPos,this);
            arrow.transform.localScale = new Vector3(150, 150, 150);
            arrow.gameObject.name = $"Arrow[{arrowMarker.roadSide}] №:{arrowMarker.id}";
            levelSettings.arrowsList.Add(arrow);
        }
        public int EditorGetCurrentStage()
        {
            for(int i = 0; i < levelSettings.stage.Length; i++)
            {
                for (int j = 0; j < levelSettings.stage[i].chartVariants.Length; j++)
                {
                    if (playableDirector.playableAsset == levelSettings.stage[i].chartVariants[j])
                    {
                        return i;
                    }
                }
            }
            return 0;
        }
        private void SaveArrows(ArrowMarker arrowMarker, ArrowMarkerTrackAsset road = null)
        {
            speedSave = levelSettings.stage[EditorGetCurrentStage()].chartSpeed;
            speedCofency = 10 / speedSave;
            if (!arrowMarker || !road) 
                return;
            SpawnArrow(arrowMarker, road.roadSide);
        }
        private void SaveArrowsOnSpeedChange()
        {
            speedSave = levelSettings.stage[EditorGetCurrentStage()].chartSpeed;
            speedCofency = 10 / speedSave;
            OnSpeedChanged?.Invoke();
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

        private void OnDestroy()
        {
            ArrowMarkerManager.instance.OnArrowCountChanged.RemoveListener(SaveArrows);
            if (Application.isPlaying)
            {
                GameStateManager.instance.OnGameStateChanged -= OnGameStateChanged;
            }
        }
    }
}