using FridayNightFunkin.Editor;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Timeline;

[TrackColor(1f, 0.5f, 0f)]
[TrackClipType(typeof(ArrowMarker))]
[Icon("Assets/FnF/Editor/icon-bf.png")]
public class ArrowMarkerTrackAsset : MarkerTrack
{
    public RoadSide roadSide;
    public float defaultSpeedMultiplier = 1;
    public uint defaultDistanceCount = 0;
    private CoroutineProcessor coroutineProcessor;
    private void Awake()
    {
        name = roadSide.ToString();
        foreach (ArrowMarker item in GetMarkers())
        {
            item.ArrowInit(this);
        }
        ArrowMarkerManager.instance.OnListCleared += OnListCleared;
    }
    private void OnDisable()
    {
        ArrowMarkerManager.instance.OnListCleared -= OnListCleared;
    }
    private void OnEnable()
    {
        ArrowMarkerManager.instance.OnListCleared += OnListCleared;
    }
    private void OnListCleared()
    {

        ArrowMarkerManager.instance.OnListCleared += OnListCleared;
        ArrowMarkerManager.instance.saveRoad[0] = null;
        ArrowMarkerManager.instance.saveRoad[1] = null;
        ArrowMarkerManager.instance.IntegrityCheck(this);
    }
    private void OnDestroy()
    {
        ArrowMarkerManager.instance.OnListCleared -= OnListCleared;
    }
}

public enum RoadSide
{
    Player,
    Enemy
}