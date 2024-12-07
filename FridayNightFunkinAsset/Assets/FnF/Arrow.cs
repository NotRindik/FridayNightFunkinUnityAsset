using FridayNightFunkin.Editor.TimeLineEditor;
using FridayNightFunkin.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArrowSide
{
    LeftArrow,
    DownArrow,
    UpArrow,
    RightArrow
}
namespace FridayNightFunkin
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
        public CharacterSide characterSide;
        private bool isHold;
        public bool isWork = true;
        public bool isViewedOnce = false;

        public ArrowMarker markerRef;

        //It's should be public, cuz it's save info from editmode to playmode
        public Vector2 startPos;

        public Vector2 endPos;

        public double startTime;

        public double endTime;

        private int arrowIndex;

        public ChartContainer chartContainer;

        internal SpriteRenderer tail;


        public float tailDistanceToArrowTakerRaw { get; private set; }
        public float tailDistance { get; private set; }

        public void Intialize(SpriteRenderer spriteRenderer,ArrowMarker arrowMarker, Vector2 startPos, Vector2 endPos, ChartContainer ñhartContainer)
        {
            isWork = true;
            isViewedOnce = false;
            spriteRendererOfArrow = spriteRenderer;
            distanceCount = arrowMarker.distanceCount;
            arrowIndex = arrowMarker.id;
            spriteRenderer.sortingOrder = spriteRenderer.sortingOrder + arrowIndex;
            this.startPos = startPos;
            this.endPos = endPos;
            endTime = arrowMarker.time;
            startTime = endTime - ((10 / LevelSettings.instance.stage[LevelSettings.instance.stageIndex].chartSpeed) * (1 / arrowMarker.speedMultiplier));
            this.chartContainer = ñhartContainer;
            markerRef = arrowMarker;
            ñhartContainer.OnSpeedChanged.AddListener(OnBaseParamChanged);
            markerRef.OnParameterChanged += OnParamChanged;
            markerRef.OnMarkerRemove += Destroy;
            chartContainer.OnSpeedChanged.AddListener(OnBaseParamChanged);
        }

        private void OnParamChanged(double time,float speedMultiplier,uint distanceCount)
        {
            startTime = time -  ((10/LevelSettings.instance.stage[LevelSettings.instance.stageIndex].chartSpeed) * (1/speedMultiplier));
            this.distanceCount = distanceCount;
            endTime = time;
        }
        private void OnBaseParamChanged()
        {
            float speedSave = LevelSettings.instance.stage[chartContainer.EditorGetCurrentStage()].chartSpeed;
            float speedCofency = 10 / speedSave;
            startTime = endTime - ((speedCofency) * (1 / markerRef.speedMultiplier));
        }

        public void Destroy(ArrowMarker arrowMarker)
        {
            markerRef.OnParameterChanged -= OnParamChanged;
            markerRef.OnMarkerRemove -= Destroy;
            chartContainer.OnSpeedChanged.RemoveListener(OnBaseParamChanged);
            LevelSettings.instance.arrowsList.Remove(this);
            DestroyImmediate(this.gameObject);
        }

        private void Update()
        {
            GenerateTail();

            if (!markerRef)
            {
                LevelSettings.instance.arrowsList.Remove(this);
                DestroyImmediate(gameObject);
                return;
            }
            if (!markerRef.IsSub(Destroy))
            {
                markerRef.OnMarkerRemove += Destroy;
            }
            if (!markerRef.IsSub(OnParamChanged))
            {
                markerRef.OnParameterChanged += OnParamChanged;
            }

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
                    var distance = arrowTakerPos - tailPos;
                    if(distance < -4)
                    {
                        tail.gameObject.SetActive(false);
                    }
                }
            }
        }

        private void GenerateTail()
        {
            if (distanceCount > 0)
            {
                if (distanceCount > Tails.Count)
                {
                    var distance = -0.20f * Mathf.Clamp((int)chartContainer.chartSpawnDistance, -1, 1);
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
                        distance -= 0.20f * Mathf.Clamp((int)chartContainer.chartSpawnDistance, -1, 1);
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
                tailDistanceToArrowTakerRaw = Camera.main.WorldToScreenPoint(endPos).y - Camera.main.WorldToScreenPoint(tail.transform.position).y;
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
            tail.transform.localPosition = new Vector2(0, -0.255f * Mathf.Clamp((int)chartContainer.chartSpawnDistance, -1, 1));
            tail.sprite = endHoldTrackSprie;
            tail.flipY = Mathf.Clamp((int)chartContainer.chartSpawnDistance, -1, 1) == 1 ? false : true;
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

        public void SetArrowHolding(bool isHold = false)
        {
            this.isHold = isHold;
        }

        private IEnumerator GiveScoreByLongArrows()
        {
            while (isHold || isWork)
            {
                yield return new WaitForSecondsRealtime(0.1f);
                if (characterSide == CharacterSide.Player)
                {
                    ScoreManager.instance.AddScore(LevelSettings.instance.addMaxScoreInLongArrow);
                    ScoreManager.instance.AddValueToSlider(LevelSettings.instance.stage[LevelSettings.instance.stageIndex].GetPlayerForce() / 2);
                    FNFUIElement.instance.UpdateUI();
                }
                else
                {
                    ScoreManager.instance.ReduceValueToSliderEnemy(LevelSettings.instance.stage[LevelSettings.instance.stageIndex].GetEnemyForce() / 2);
                    FNFUIElement.instance.UpdateUI();
                }
            } 
        }

        public void SetStartPos(Vector2 pos)
        {
            startPos = pos;
        }
        public void SetEndPos(Vector2 pos)
        {
            endPos = pos;
        }
    }
}
