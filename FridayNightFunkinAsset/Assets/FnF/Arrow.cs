using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FridayNightFunkin;
using FridayNightFunkin.UI;

public enum ArrowSide
{
    LeftArrow,
    DownArrow,
    UpArrow,
    RightArrow
}
[ExecuteAlways]
public class Arrow : MonoBehaviour
{
    [SerializeField] internal ArrowSide arrowSide;
    private const float ACURANCY_DISTANCE_DEFAULT = -0.7f;
    [SerializeField] internal uint distanceCount;
    [SerializeField] SpriteRenderer holdTrack;
    [SerializeField] Sprite holdTrackSprite;
    [SerializeField] Sprite endHoldTrackSprie;
    List<SpriteRenderer> instances = new List<SpriteRenderer>();
    public float takerCheakRadius;
    public LayerMask arrowTakerLayer;
    public SpriteRenderer spriteRendererOfArrow;
    public CharacterSide characterSide;
    private bool isHold;
    public bool isWork = true;
    public bool isViewedOnce = false;

    public Vector2 startPos { get; private set; }

    public Vector2 endPos { get; private set; }

    public double startTime { get; private set; }

    public double endTime { get; private set; }


    public int arrowIndex;


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

#if UNITY_EDITOR
    private void Update()
    {
        GenerateTail();
        if (tailDistanceToArrowTakerRaw > 0 && isHold && distanceCount > 0)
        {
            isWork = false;
            gameObject.SetActive(false);
        }
    }

    private void GenerateTail()
    {
        if (distanceCount > 0)
        {
            if (distanceCount > instances.Count)
            {
                var distance = -0.20f;
                instances = new List<SpriteRenderer>();
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
                    instances.Add(instance);
                }
                tail = Instantiate(holdTrack, transform.position, Quaternion.identity, instances[instances.Count - 1].transform);
                tail.transform.localPosition = new Vector2(0, -0.255f);
                tail.sprite = endHoldTrackSprie;
            }
            else if (distanceCount < instances.Count)
            {
                var baseInstancesCount = instances.Count;
                for (int i = 0; i < baseInstancesCount - distanceCount; i++)
                {
                    instances.RemoveAt(instances.Count - 1);
                    DestroyImmediate(transform.GetChild(transform.childCount - 1).gameObject);
                }
                tail = Instantiate(holdTrack, transform.position, Quaternion.identity, instances[instances.Count - 1].transform);
                tail.transform.localPosition = new Vector2(0, -0.255f);
                tail.sprite = endHoldTrackSprie;
            }

            tailDistanceToArrowTakerRaw = endPos.y - tail.transform.position.y;
            tailDistance = Mathf.Abs(tail.transform.position.y - transform.position.y);
        }
        else if (distanceCount == 0)
        {
            var baseChildCount = instances.Count;
            instances = new List<SpriteRenderer>();
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
            if (isHold)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position + new Vector3(0, 0.0001f, 0), tail.transform.position + new Vector3(0, -0.38f, 0));
            }
            else
            {
                Gizmos.color = new Color(255, 0, 0);
                Gizmos.DrawLine(transform.position + new Vector3(0, 0.0001f, 0), tail.transform.position + new Vector3(0, -0.38f, 0));
            }
        }
        Gizmos.color = Color.blue;
    }

#endif
    public void TakeArrow(bool isHold)
    {
        this.isHold = isHold;
        if (distanceCount == 0)
        {
            isWork = false;
            gameObject.SetActive(false);
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
    //public void TakeLongArrow(float distanceForAccurancy, bool isHold)
    //{
    //    this.isHold = isHold;
    //    isWork = false;
    //    IsTakingLongArrow(); 
    //}
    //public void TakeLongArrow(bool isHold, List<Arrow> arrowsRef)
    //{
    //    isWork = false;
    //    this.isHold = isHold;
    //    IsTakingLongArrow(arrowsRef);
    //}

    //public bool IsTakingLongArrow()
    //{
    //    if (isHold)
    //    {
    //        if (!IsTakerUnderArrow())
    //        {
    //            isWork = false;
    //            isHold = false;
    //            ScoreManager.instance.AddCombo();
    //            ScoreManager.instance.AddValueToSlider(LevelSettings.instance.playerForce + distanceCount);
    //            FNFUIElement.instance.UpdateUI();
    //            gameObject.SetActive(false);

    //            return true; 
    //        }
    //        else
    //        {
    //            spriteRendererOfArrow.color = new Color(spriteRendererOfArrow.color.r, spriteRendererOfArrow.color.g, spriteRendererOfArrow.color.b, 0f);
    //        }
    //    }

    //    return false;
    //}

    //public bool IsTakingLongArrow(List<Arrow> arrowsRef)
    //{
    //    if (isHold)
    //    {
    //        if (!IsTakerUnderArrow())
    //        {
    //            isHold = false;
    //            isWork = false;
    //            ScoreManager.instance.ReduceValueToSliderEnemy(LevelSettings.instance.enemyForce);
    //            arrowsRef.Remove(this);
    //            gameObject.SetActive(false);

    //            return true;
    //        }
    //        else
    //        {
    //            spriteRendererOfArrow.color = new Color(spriteRendererOfArrow.color.r, spriteRendererOfArrow.color.g, spriteRendererOfArrow.color.b, 0f);
    //        }
    //    }

    //    return false;
    //}
    //public bool IsTakerUnderArrow()
    //{
    //    Collider2D hit =  Physics2D.OverlapCircle(transform.position, takerCheakRadius, arrowTakerLayer);

    //    if (distanceCount != 0 && tail != null)
    //        hit = Physics2D.OverlapArea(transform.position + new Vector3(0, 0.0001f, 0), tail.transform.position + new Vector3(0, -0.38f, 0), arrowTakerLayer);


    //    if (hit != null)
    //    {
    //        return true;
    //    }

    //    return false;
    //}
}
