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

    public Vector2 startPos;

    public Vector2 endPos;

    public double startTime;

    public double endTime;

    public int arrowIndex;

    public void Intialize(SpriteRenderer spriteRenderer,uint distanceCount,Vector2 startPos, Vector2 endPos, double startTime, double endTime,int arrowIndex)
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
    internal SpriteRenderer tail;

#if UNITY_EDITOR
    private void Update()
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
                tail = Instantiate(holdTrack, transform.position, Quaternion.identity, instances[instances.Count-1].transform);
                tail.transform.localPosition = new Vector2(0, -0.255f);
                tail.gameObject.layer = LayerMask.NameToLayer("EndOfArrow");
                var collider = tail.gameObject.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
                collider.size = new Vector2(0.25f, 0.25f);
                tail.sprite = endHoldTrackSprie;
            }
            else if (distanceCount < instances.Count)
            {
                var baseInstancesCount = instances.Count;
                for (int i = 0; i < baseInstancesCount - distanceCount; i++)
                {
                    instances.RemoveAt(instances.Count - 1);
                    DestroyImmediate(transform.GetChild(transform.childCount-1).gameObject);
                }
                tail = Instantiate(holdTrack, transform.position, Quaternion.identity, instances[instances.Count - 1].transform);
                tail.transform.localPosition = new Vector2(0, -0.255f);

                tail.gameObject.layer = LayerMask.NameToLayer("EndOfArrow");
                tail.sprite = endHoldTrackSprie;
            }
        }
        else if(distanceCount == 0)
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
    public void TakeArrow()
    {
        isWork = false;
        gameObject.SetActive(false);
    }
    public void TakeLongArrow(float distanceForAccurancy, bool isHold)
    {
        this.isHold = isHold;
        isWork = false;
        IsTakingLongArrow(); 
    }
    public void TakeLongArrow(bool isHold, List<Arrow> arrowsRef)
    {
        isWork = false;
        this.isHold = isHold;
        IsTakingLongArrow(arrowsRef);
    }

    public bool IsTakingLongArrow()
    {
        if (isHold)
        {
            if (!IsTakerUnderArrow())
            {
                isWork = false;
                isHold = false;
                ScoreManager.instance.AddCombo();
                ScoreManager.instance.AddValueToSlider(LevelSettings.instance.playerForce + distanceCount);
                FNFUIElement.instance.UpdateUI();
                gameObject.SetActive(false);

                return true; 
            }
            else
            {
                spriteRendererOfArrow.color = new Color(spriteRendererOfArrow.color.r, spriteRendererOfArrow.color.g, spriteRendererOfArrow.color.b, 0f);
            }
        }

        return false;
    }

    public bool IsTakingLongArrow(List<Arrow> arrowsRef)
    {
        if (isHold)
        {
            if (!IsTakerUnderArrow())
            {
                isHold = false;
                isWork = false;
                ScoreManager.instance.ReduceValueToSliderEnemy(LevelSettings.instance.enemyForce);
                arrowsRef.Remove(this);
                gameObject.SetActive(false);

                return true;
            }
            else
            {
                spriteRendererOfArrow.color = new Color(spriteRendererOfArrow.color.r, spriteRendererOfArrow.color.g, spriteRendererOfArrow.color.b, 0f);
            }
        }

        return false;
    }
    public bool IsTakerUnderArrow()
    {
        Collider2D hit =  Physics2D.OverlapCircle(transform.position, takerCheakRadius, arrowTakerLayer);

        if (distanceCount != 0 && tail != null)
            hit = Physics2D.OverlapArea(transform.position + new Vector3(0, 0.0001f, 0), tail.transform.position + new Vector3(0, -0.38f, 0), arrowTakerLayer);


        if (hit != null)
        {
            return true;
        }

        return false;
    }
}
