using FridayNightFunkin.Editor;
using FridayNightFunkin.Editor.TimeLineEditor;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

[TrackColor(1f, 0.5f, 0f)]
[TrackClipType(typeof(ArrowMarker))]
[TrackBindingType(typeof(ChartPlayBack))]
[Icon("Assets/FnF/Editor/icon-bf.png")]
public class ArrowMarkerTrackAsset : MarkerTrack
{
    public RoadSide roadSide;
    public bool isActive = false;
    public LevelDataWindow levelDataWindow => EditorWindow.GetWindow<LevelDataWindow>();
    public List<ArrowMarker> arrowMarkers = new List<ArrowMarker>();

    public int arrowCurrentIndex = 0;

    public ChartPlayBack chartPlayBack;

    private void OnDisable()
    {

    }

    private void OnEnable()
    {
        foreach (ArrowMarker item in GetMarkers())
        {
            item.ArrowInit(this);
        }
    }
    public void AddMarkersToList(ArrowMarker arrowMarker)
    {
        if (!arrowMarker)
            return;

        AddMarker(arrowMarker);
        chartPlayBack.SaveArrows(arrowMarker, this);
    }
    public void AddMarker(ArrowMarker marker)
    {
        if (!arrowMarkers.Contains(marker))
        {
            arrowCurrentIndex++;
            marker.OnMarkerRemove += RemoveMarker;
            arrowMarkers.Add(marker);
        }
    }

    public void RemoveMarker(ArrowMarker marker)
    {
        arrowMarkers.Remove(marker);
    }


    private void OnListCleared()
    {
        IsActive();
        if (!isActive)
            return;
    }
    private void OnDestroy()
    {
       
    }

    private void IsActive()
    {
        if(levelDataWindow == null)
            isActive = false;

        if (levelDataWindow.levelData.stage[levelDataWindow.selectedStageIndex].chartVariants[levelDataWindow.selectedChartVar] == parent)
        {
            isActive = true;
        }
        else
        {
            isActive = false;
        }
    }
}
public enum RoadSide
{
    Player,
    Enemy
}