using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;

namespace FridayNightFunkin.Editor.TimeLineEditor
{
    [ExecuteAlways]
    public class СhartContainer : MonoBehaviour
    {
        public Arrow[] arrows;
        private double time;
        public int arrowsLayer;
        public PlayableDirector playableDirector;
        private List<ArrowMarker> markers = new List<ArrowMarker>();
        public RectTransform chartRoad;
        public float chartSpawnDistance = 10;

        [SerializeField]private bool isSaveCharts = false;
        private int[] markerCount = new int[] { 0, 0 };
        private float speedSave;

        private List<uint> distanceCount = new List<uint>();
        private List<double> arrowsTime = new List<double>();   
        private List<TrackAsset> signalTracks;

        private LevelSettings levelSettings;

        private float speedCofency;

        public bool playOnStart = true;

        public bool isReLoadChart;

        public bool resetList;

        public int playerArrowCount;
        public int enemyArrowCount;

        private void Start()
        {
            TryGetComponent(out PlayableDirector playableDirector);
            this.playableDirector = playableDirector;
            levelSettings = LevelSettings.instance;
            if (Application.isPlaying && levelSettings)
            {   
                GameStateManager.instance.OnGameStateChanged += OnGameStateChanged;
                if (playOnStart)
                {
                    StartLevel();
                }
            }
        }

        public void ReloadChart()
        {
            isReLoadChart = true;
            isSaveCharts = true;
        }

        public void StartLevel()
        {
            if (PlayerPrefs.HasKey("Difficult"))
            {
                playableDirector.playableAsset = levelSettings.stage[levelSettings.stageIndex].chartVariants[PlayerPrefs.GetInt("Difficult")];
                ReloadChart();
            }
            else
            {
                playableDirector.playableAsset = levelSettings.stage[levelSettings.stageIndex].chartVariants[0];
            }
            playableDirector.Play();
        }

        private void Update()
        {
            time = playableDirector.time;
            playerArrowCount = ArrowMarkerManager.instance.playerArrowCount;
            enemyArrowCount = ArrowMarkerManager.instance.enemyArrowCount;

            if (!ArrowMarkerManager.instance.IsMethodSubscribed(SaveArrows))
            {
                ArrowMarkerManager.instance.OnArrowCountChanged += SaveArrows;
            }

            if (resetList)
            {
                for (int i = 0; i < transform.childCount;)
                {
                    DestroyImmediate(transform.GetChild(i).gameObject);
                }
                LevelSettings.instance.arrowsList.Clear();
                ArrowMarkerManager.instance.LoadDataFromRoad();
                resetList = false;
            }

            if(playableDirector.duration - 1 < time && Application.isPlaying)
            {
                playableDirector.time = 0;
                if(LevelSettings.instance.stageIndex == 0) 
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

            }
            else
            {
                levelSettings = LevelSettings.instance;
            }
        }
        public void SpawnArrow(ArrowMarker arrowMarker, double time, uint distanceCount, RoadSide roadSide)
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
            arrow.markerRef = arrowMarker;
            arrow.Intialize(arrow.GetComponent<SpriteRenderer>(), distanceCount, startPos, endPos, time - (speedCofency * (1/arrowMarker.speedMultiplier)), time, arrowMarker.id);
            arrow.transform.localScale = new Vector3(150, 150, 150);
            arrow.gameObject.name = $"Arrow[{arrowMarker.roadSide}] №:{arrowMarker.id}";
            levelSettings.arrowsList.Add(arrow);
        }
        private void SaveArrows(ArrowMarker arrowMarker, ArrowMarkerTrackAsset road = null)
        {
            speedSave = levelSettings.stage[levelSettings.stageIndex].chartSpeed;
            speedCofency = 10 / speedSave;
            if (!arrowMarker || !road)
                return;
            SpawnArrow(arrowMarker, arrowMarker.time, arrowMarker.distanceCount, road.roadSide);
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
            ArrowMarkerManager.instance.OnArrowCountChanged -= SaveArrows;
            if (Application.isPlaying)
            {
                GameStateManager.instance.OnGameStateChanged -= OnGameStateChanged;
            }
        }
    }
}