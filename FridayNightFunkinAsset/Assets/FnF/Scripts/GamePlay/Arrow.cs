using System;
using FridayNightFunkin.Calculations;
using FridayNightFunkin.Editor.TimeLineEditor;
using FridayNightFunkin.UI;
using System.Collections;
using System.Collections.Generic;
using FnF.Scripts.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

public enum ArrowSide
{
    LeftArrow,
    DownArrow,
    UpArrow,
    RightArrow
}
namespace FridayNightFunkin.GamePlay
{
    [ExecuteAlways]
    public class Arrow : MonoBehaviour
    {
        [SerializeField] internal ArrowSide arrowSide;
        [SerializeField] internal uint distanceCount => markerRef != null ? markerRef.distanceCount : 0;
        [SerializeField] SpriteRenderer holdTrack;
        [SerializeField] Sprite holdTrackSprite;
        [SerializeField] Sprite endHoldTrackSprie;
        List<SpriteRenderer> Tails = new List<SpriteRenderer>();
        public SpriteRenderer spriteRendererOfArrow;
        public RoadSide roadSide;
        private bool isHold;
        public bool isWork = true;
        public bool isViewedOnce = false;

        public ArrowMarker markerRef;
        
        public Vector2 StartPos => new Vector3(arrowTaker.transform.position.x, arrowTaker.transform.position.y - ChartSpawnDistance * (camera.orthographicSize / 5), 0);

        public Vector2 EndPos => arrowTaker.transform.position;

        public double StartTime => markerRef ? EndTime - ((10 / chartPlayback.levelData.stage[chartPlayback.levelData.selectedStageIndex].ChartSpeed) * (1 / markerRef.speedMultiplier)): 0;

        public double EndTime => markerRef ? markerRef.time : 0;

        private int _arrowIndex;

        public ChartPlayBack chartPlayback;

        internal SpriteRenderer Tail;

        public Transform arrowTaker;

        private bool _isPause;

        private float ChartSpawnDistance => chartPlayback.chartSpawnDistance;
        public float TailDistanceToArrowTakerRaw  => camera.WorldToScreenPoint(EndPos).y - camera.WorldToScreenPoint(Tail.transform.position).y * Mathf.Clamp(Mathf.Round(ChartSpawnDistance),-1,1);
        public float TailDistance { get; private set; }

        public Camera camera;

        public void Intialize(ArrowMarker arrowMarker,Transform arrowTaker, ChartPlayBack chartPlayBack)
        {
            camera = Camera.main;
            isWork = true;
            isViewedOnce = false;
            spriteRendererOfArrow = GetComponent<SpriteRenderer>();
            _arrowIndex = arrowMarker.id;
            spriteRendererOfArrow.sortingOrder += _arrowIndex;
            this.arrowTaker = arrowTaker;
            this.chartPlayback = chartPlayBack;
            markerRef = arrowMarker;

            GameStateManager.instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void Update()
        {
            if(!markerRef && gameObject)
                DestroyImmediate(gameObject);
            GenerateTail();

            if (!Application.isPlaying)
                return;

            if (Tail)
            {
                if (TailDistanceToArrowTakerRaw < 50 && isHold && distanceCount > 0)
                {
                    isWork = false;
                    gameObject.SetActive(false);
                }
                if (isHold)
                {
                    foreach (var tail in Tails)
                    {
                        if (!tail.gameObject.activeInHierarchy)
                            continue;
                        var tailPos = camera.WorldToScreenPoint(tail.transform.position).y;
                        var arrowTakerPos = camera.WorldToScreenPoint(EndPos).y;
                        var distance = arrowTakerPos - tailPos * Mathf.Clamp(Mathf.Round(ChartSpawnDistance), -1, 1);
                        if (distance < -4 * Mathf.Clamp(Mathf.Round(ChartSpawnDistance), -1, 1))
                        {
                            tail.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        public void OnDestroy()
        {
            chartPlayback.chartContainer.arrowsList[roadSide].Remove(this);
            GameStateManager.instance.OnGameStateChanged -= OnGameStateChanged;
        }

        private void GenerateTail()
        {
            if (distanceCount > 0)
            {
                if (distanceCount > Tails.Count)
                {
                    var distance = -0.20f * Mathf.Clamp((int)ChartSpawnDistance, -1, 1);
                    Tails = new List<SpriteRenderer>();
                    var baseChildCount = transform.childCount;
                    for (int i = 0; i < baseChildCount; i++)
                    {
                        DestroyImmediate(transform.GetChild(0).gameObject);
                    }

                    for (int count = 0; count > -distanceCount; count -= 1)
                    {
                        var instance = Instantiate(holdTrack, transform.position, Quaternion.identity, transform);
                        instance.transform.localPosition = new Vector2(0, distance);
                        instance.sprite = holdTrackSprite;
                        distance -= 0.20f * Mathf.Clamp((int)ChartSpawnDistance, -1, 1);
                        Tails.Add(instance);
                    }
                    spawnTail();
                }
                else if (distanceCount < Tails.Count)
                {
                    var baseInstancesCount = Tails.Count;
                    for (int i = 0; i < baseInstancesCount - distanceCount; i++)
                    {
                        Tails.RemoveAt(Tails.Count - 1);
                        DestroyImmediate(transform.GetChild(transform.childCount - 1).gameObject);
                    }
                    Tail = Instantiate(holdTrack, transform.position, Quaternion.identity, Tails[^1].transform);
                    spawnTail();
                }
                
                TailDistance = Mathf.Abs(camera.WorldToScreenPoint(Tail.transform.position).y - camera.WorldToScreenPoint(transform.position).y);
            }
            else if (distanceCount == 0)
            {
                var baseChildCount = Tails.Count;
                Tails = new List<SpriteRenderer>();
                for (int i = 0; i < baseChildCount; i++)
                {
                    DestroyImmediate(transform.GetChild(0).gameObject);
                }
            }
        }

        private void spawnTail()
        {
            Tail = Instantiate(holdTrack, transform.position, Quaternion.identity, Tails[^1].transform);
            Tail.transform.localPosition = new Vector2(0, -0.255f * Mathf.Clamp((int)ChartSpawnDistance, -1, 1));
            Tail.sprite = endHoldTrackSprie;
            Tail.flipY = Mathf.Clamp((int)ChartSpawnDistance, -1, 1) != 1;
        }
        
        public void TakeArrow(bool isHold = false)
        {
            this.isHold = isHold;
            switch (distanceCount)
            {
                case 0:
                    isWork = false;
                    gameObject.SetActive(false);
                    break;
                default:
                    spriteRendererOfArrow.color = new Color(255, 255, 255, 0);
                    StartCoroutine(GiveScoreByLongArrows());
                    break;
            }
        }

        public void MoveArrowByTime(double timelineTime)
        {
            if(!this)
                return;

            if (!markerRef && gameObject)
            {
                DestroyImmediate(gameObject);
                return;
            }

            if (Math.Abs(EndTime - StartTime) < double.Epsilon)
            {
                Debug.LogError($"endTime or startTime equals to zero");
                transform.position = Vector2.zero;
            }
            double speed = (EndPos.y - StartPos.y) / (EndTime - StartTime);

            Vector2 arrowPos = new Vector2(StartPos.x, StartPos.y + (float)(speed * (timelineTime - StartTime)));

            transform.position = arrowPos;
        }

        public void SetArrowHolding(bool isHold = false)
        {
            this.isHold = isHold;
        }

        private IEnumerator GiveScoreByLongArrows()
        {
            while (isHold && isWork)
            {
                yield return new WaitForSecondsRealtime(0.1f);
                if (!_isPause)
                {
                    if (roadSide == RoadSide.Player)
                    {
                        G.Instance.Get<StatisticManager>().AddScore(chartPlayback.levelData.addMaxScoreInLongArrow);
                        G.Instance.Get<HealthBar>().ModifyValue(chartPlayback.levelData.stage[ChartPlayBack.CurrentStageIndex].GetPlayerForce() / 2);
                    }
                    else
                    {
                        G.Instance.Get<HealthBar>().ModifyValue(-chartPlayback.levelData.stage[ChartPlayBack.CurrentStageIndex].GetEnemyForce() / 2);
                    }
                }
            } 
        }
        
        private void OnGameStateChanged(GameState currenState)
        {
            _isPause = currenState == GameState.Paused;
        }
    }
}
