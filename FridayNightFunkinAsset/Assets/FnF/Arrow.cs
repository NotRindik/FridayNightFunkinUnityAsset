using FridayNightFunkin.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        //It's should be public, cuz it's save info from editmode to playmode
        public Vector2 startPos;

        public Vector2 endPos;

        public double startTime;

        public double endTime;


        private int arrowIndex;


        internal SpriteRenderer tail;


        public float tailDistanceToArrowTakerRaw { get; private set; }
        public float tailDistance { get; private set; }

        public void Intialize(SpriteRenderer spriteRenderer, uint distanceCount, Vector2 startPos, Vector2 endPos, double startTime, double endTime, int arrowIndex)
        {
            isWork = true;
            isViewedOnce = false;
            spriteRendererOfArrow = spriteRenderer;
            this.distanceCount = distanceCount;
            this.arrowIndex = arrowIndex;
            spriteRenderer.sortingOrder = spriteRenderer.sortingOrder + arrowIndex;
            this.startPos = startPos;
            this.endPos = endPos;
            this.startTime = startTime;
            this.endTime = endTime;
        }
        private void Update()
        {
            GenerateTail();

            if (!Application.isPlaying)
                return;
            if (tailDistanceToArrowTakerRaw < 0 && isHold && distanceCount > 0)
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
                    var distance = endPos.y + 4 - tail.transform.position.y;
                    if(distance < tail.transform.position.y)
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
                    var distance = -0.20f;
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
                        distance -= 0.20f;
                        Tails.Add(instance);
                    }
                    tail = Instantiate(holdTrack, transform.position, Quaternion.identity, Tails[Tails.Count - 1].transform);
                    tail.transform.localPosition = new Vector2(0, -0.255f);
                    tail.sprite = endHoldTrackSprie;
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
                    tail.transform.localPosition = new Vector2(0, -0.255f);
                    tail.sprite = endHoldTrackSprie;
                }

                tailDistanceToArrowTakerRaw = endPos.y - tail.transform.position.y;
                tailDistance = Mathf.Abs(tail.transform.position.y - transform.position.y);
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
