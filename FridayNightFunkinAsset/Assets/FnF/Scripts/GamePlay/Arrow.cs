using System;
using FridayNightFunkin.Calculations;
using FridayNightFunkin.Editor.TimeLineEditor;
using FridayNightFunkin.UI;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] internal uint distanceCount;
        [SerializeField] SpriteRenderer holdTrack;
        [SerializeField] Sprite holdTrackSprite;
        [SerializeField] Sprite endHoldTrackSprie;
        List<SpriteRenderer> Tails = new List<SpriteRenderer>();
        public float takerCheakRadius;
        public LayerMask arrowTakerLayer;
        public SpriteRenderer spriteRendererOfArrow;
        public RoadSide roadSide;
        private bool isHold;
        public bool isWork = true;
        public bool isViewedOnce = false;

        public ArrowMarker markerRef;

        //It's should be public, cuz it's save info from editmode to playmode
        public Vector2 startPos
        {
            get
            {
                
                return new Vector3(arrowTaker.transform.position.x, arrowTaker.transform.position.y - _chartSpawnDistance * (camera.orthographicSize / 5), 0);
            }
        }

        public Vector2 endPos => arrowTaker.transform.position;

        public double startTime => markerRef ? endTime - ((10 / chartPlayback.levelData.stage[chartPlayback.levelData.selectedStageIndex].chartSpeed) * (1 / markerRef.speedMultiplier)): 0;

        public double endTime => markerRef ? markerRef.time : 0;

        private int arrowIndex;

        public ChartPlayBack chartPlayback;

        internal SpriteRenderer tail;

        [FormerlySerializedAs("_arrowTaker")] public Transform arrowTaker;

        [SerializeField] private float _chartSpawnDistance;
        public float tailDistanceToArrowTakerRaw { get; private set; }
        public float tailDistance { get; private set; }

        public Camera camera;

        public void Intialize(ArrowMarker arrowMarker,Transform arrowTaker,float chartSpawnDistance, ChartPlayBack chartPlayBack)
        {
            camera = Camera.main;
            isWork = true;
            isViewedOnce = false;
            spriteRendererOfArrow = GetComponent<SpriteRenderer>();
            distanceCount = arrowMarker.distanceCount;
            arrowIndex = arrowMarker.id;
            spriteRendererOfArrow.sortingOrder += arrowIndex;
            this.arrowTaker = arrowTaker;
            _chartSpawnDistance = chartSpawnDistance;
            this.chartPlayback = chartPlayBack;
            markerRef = arrowMarker;
            markerRef.arrow = this;
        }

        public void OnParamChanged(double time,float speedMultiplier,uint distanceCount)
        {
            this.distanceCount = distanceCount;
            float speedSave = chartPlayback.levelData.stage[chartPlayback.levelData.selectedStageIndex].chartSpeed;
            float speedCofency = 10 / speedSave;
        }

        private void Update()
        {
            if(!markerRef && gameObject)
                DestroyImmediate(gameObject);
            GenerateTail();

            if (!Application.isPlaying)
                return;


            if (tailDistanceToArrowTakerRaw < 50 && isHold && distanceCount > 0)
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
                    var tailPos = Camera.main.WorldToScreenPoint(tail.transform.position).y;
                    var arrowTakerPos = Camera.main.WorldToScreenPoint(endPos).y;
                    var distance = arrowTakerPos - tailPos * Mathf.Clamp(Mathf.Round(chartPlayback.chartSpawnDistance), -1, 1);
                    if(distance < -4 * Mathf.Clamp(Mathf.Round(chartPlayback.chartSpawnDistance), -1, 1))
                    {
                        tail.gameObject.SetActive(false);
                    }
                }
            }
        }

        public void OnDestroy()
        {
            chartPlayback.ChartContainer.arrowsList[roadSide].Remove(this);
        }

        private void GenerateTail()
        {
            if (distanceCount > 0)
            {
                if (distanceCount > Tails.Count)
                {
                    var distance = -0.20f * Mathf.Clamp((int)chartPlayback.chartSpawnDistance, -1, 1);
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
                        distance -= 0.20f * Mathf.Clamp((int)chartPlayback.chartSpawnDistance, -1, 1);
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
                    tail = Instantiate(holdTrack, transform.position, Quaternion.identity, Tails[Tails.Count - 1].transform);
                    spawnTail();
                }
                tailDistanceToArrowTakerRaw = Camera.main.WorldToScreenPoint(endPos).y - Camera.main.WorldToScreenPoint(tail.transform.position).y * Mathf.Clamp(Mathf.Round(chartPlayback.chartSpawnDistance),-1,1);
                tailDistance = Mathf.Abs(Camera.main.WorldToScreenPoint(tail.transform.position).y - Camera.main.WorldToScreenPoint(transform.position).y);
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
            tail = Instantiate(holdTrack, transform.position, Quaternion.identity, Tails[Tails.Count - 1].transform);
            tail.transform.localPosition = new Vector2(0, -0.255f * Mathf.Clamp((int)chartPlayback.chartSpawnDistance, -1, 1));
            tail.sprite = endHoldTrackSprie;
            tail.flipY = Mathf.Clamp((int)chartPlayback.chartSpawnDistance, -1, 1) == 1 ? false : true;
        }

        private void OnDrawGizmos()
        {
            if (tail != null && transform != null)
            {
                switch (isHold)
                {
                    case true:
                        Gizmos.color = Color.green;
                        Gizmos.DrawLine(transform.position + new Vector3(0, 0.0001f, 0), tail.transform.position + new Vector3(0, -0.38f, 0));
                        break;
                    default:
                        Gizmos.color = new Color(255, 0, 0);
                        Gizmos.DrawLine(transform.position + new Vector3(0, 0.0001f, 0), tail.transform.position + new Vector3(0, -0.38f, 0));
                        break;
                }
            }
            Gizmos.color = Color.blue;
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
            if(!markerRef && gameObject)
                DestroyImmediate(gameObject);
            
            if (Math.Abs(endTime - startTime) < double.Epsilon)
            {
                Debug.LogError($"endTime or startTime equals to zero");
                transform.position = Vector2.zero;
            }
            double speed = (endPos.y - startPos.y) / (endTime - startTime);

            Vector2 arrowPos = new Vector2(startPos.x, startPos.y + (float)(speed * (timelineTime - startTime)));

            transform.position = arrowPos;
        }

        public void SetArrowHolding(bool isHold = false)
        {
            this.isHold = isHold;
        }

        private IEnumerator GiveScoreByLongArrows()
        {
            while (isHold || isWork)
            {
                yield return new WaitForSecondsRealtime(0.1f);
                if (roadSide == RoadSide.Player)
                {
                    ScoreManager.instance.AddScore(chartPlayback.levelData.addMaxScoreInLongArrow);
                    ScoreManager.instance.AddValueToSlider(chartPlayback.levelData.stage[chartPlayback.currentStageIndex].GetPlayerForce() / 2);
                    FNFUIElement.instance.UpdateUI();
                }
                else
                {
                    ScoreManager.instance.ReduceValueToSliderEnemy(chartPlayback.levelData.stage[chartPlayback.currentStageIndex].GetEnemyForce() / 2);
                    FNFUIElement.instance.UpdateUI();
                }
            } 
        }
    }
}
