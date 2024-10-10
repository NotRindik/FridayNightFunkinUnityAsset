using FridayNightFunkin.Editor;
using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.Timeline;
public class ArrowMarker : Marker, INotificationOptionProvider
{
    [SerializeField] protected bool emitOnce;
    [SerializeField] protected bool emitInEditor;
    [SerializeField] public uint distanceCount;
    private uint _distanceCount;

    public int id;
    private PropertyName objectID => new PropertyName();
    public RoadSide roadSide = RoadSide.Player;

    public ArrowSide arrowSide;

    public delegate void OnAddMarker();
    public event OnAddMarker OnMarkerAdd;

    public delegate void OnRemoveMarker(ArrowMarker arrowMarker);
    public event OnRemoveMarker OnMarkerRemove;
    public ArrowMarkerTrackAsset arrowMarkerParent { private set; get; }

    public event Action<double,float,uint> OnParameterChanged;

    public float speedMultiplier = 1;
    private float _speedMultiplier;

    private double currentTime;

    public override void OnInitialize(TrackAsset aPent)
    {
        OnMarkerAdd?.Invoke();
        EditorApplication.update += Update;
        if (aPent is ArrowMarkerTrackAsset)
        {
            id = aPent.GetMarkerCount();
            arrowMarkerParent = aPent as ArrowMarkerTrackAsset;
            roadSide = arrowMarkerParent.roadSide;
            speedMultiplier = arrowMarkerParent.defaultSpeedMultiplier;
            distanceCount = arrowMarkerParent.defaultDistanceCount;
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
            OnParameterChanged?.Invoke(currentTime, _speedMultiplier, _distanceCount);
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

    public void ArrowInit(ArrowMarkerTrackAsset arrowMarkerParent)
    {
        this.arrowMarkerParent = arrowMarkerParent;
        roadSide = arrowMarkerParent.roadSide;
    }
    protected void OnDestroy()
    {
        OnMarkerRemove?.Invoke(this);
    }
    NotificationFlags INotificationOptionProvider.flags =>
       (emitOnce ? NotificationFlags.TriggerOnce : default) |
       (emitInEditor ? NotificationFlags.TriggerInEditMode : default);
}
