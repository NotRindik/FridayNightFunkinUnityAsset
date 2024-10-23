using FridayNightFunkin.Editor;
using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.Timeline;
public class ArrowMarker : Marker
{
    [SerializeField] public uint distanceCount;
    private uint _distanceCount;

    public int id;
    protected PropertyName objectID => new PropertyName();
    public RoadSide roadSide = RoadSide.Player;

    public ArrowSide arrowSide;

    public delegate void OnAddMarker();
    public event OnAddMarker OnMarkerAdd;

    public delegate void OnRemoveMarker(ArrowMarker arrowMarker);
    public event OnRemoveMarker OnMarkerRemove;
    public ArrowMarkerTrackAsset arrowMarkerParent { private set; get; }

    public delegate void OnParameterChange(double time, float speed , uint distance);

    public event OnParameterChange OnParameterChanged;

    public float speedMultiplier = 1;
    private float _speedMultiplier;

    private double currentTime;

    public void OnEnable()
    {
        OnMarkerAdd?.Invoke();
        EditorApplication.update += Update;
        if (parent is ArrowMarkerTrackAsset)
        {
            arrowMarkerParent = parent as ArrowMarkerTrackAsset;
            id = arrowMarkerParent.roadSide == RoadSide.Player ? ArrowMarkerManager.instance.playerArrowCount : ArrowMarkerManager.instance.enemyArrowCount;
            roadSide = arrowMarkerParent.roadSide;
            _speedMultiplier = speedMultiplier;
            _distanceCount = distanceCount;
            ArrowMarkerManager.instance.AddArowMarker(this, arrowMarkerParent);
        }
    }

    private void Update()
    {

        if (_speedMultiplier != speedMultiplier)
        {
            _speedMultiplier = speedMultiplier;
            OnParameterChanged?.Invoke(currentTime, Mathf.Abs(_speedMultiplier) != 0 ? Mathf.Abs(_speedMultiplier) : 0.0001f, _distanceCount);
        }
        if (_distanceCount != distanceCount)
        {
            _distanceCount = distanceCount;
            OnParameterChanged?.Invoke(currentTime, _speedMultiplier, _distanceCount);
        }

        if (Math.Abs(currentTime - time) > double.Epsilon)
        {
            currentTime = time;
            OnParameterChanged?.Invoke(currentTime, _speedMultiplier, _distanceCount);
        }
    }
    public bool IsSub(OnRemoveMarker method)
    {
        if (OnMarkerRemove == null) return false;

        foreach (var d in OnMarkerRemove.GetInvocationList())
        {
            if (d.Method == method.Method && d.Target == method.Target)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsSub(OnParameterChange method)
    {
        if (OnParameterChanged == null) return false;

        foreach (var d in OnParameterChanged.GetInvocationList())
        {
            if (d.Method == method.Method && d.Target == method.Target)
            {
                return true;
            }
        }
        return false;
    }

    public void ArrowInit(ArrowMarkerTrackAsset arrowMarkerParent)
    {
        this.arrowMarkerParent = arrowMarkerParent;
        roadSide = arrowMarkerParent.roadSide;
    }
    protected void OnDestroy()
    {
        OnMarkerRemove?.Invoke(this);
        EditorApplication.update -= Update;
    }
}
