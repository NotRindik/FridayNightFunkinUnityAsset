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
        if (!chartPlayBack)
        {
            return;
        }

            AddMarker(arrowMarker);
        chartPlayBack.SaveArrows(arrowMarker, this);
    }
    public void AddMarker(ArrowMarker marker)
    {
        if (!arrowMarkers.Contains(marker))
        {
            arrowCurrentIndex++;
            arrowMarkers.Add(marker);
        }
    }

    public void RemoveMarker(ArrowMarker marker)
    {
        arrowMarkers.Remove(marker);
    }


    private void OnListCleared()
    {
        arrowCurrentIndex = 0;
    }
    public void LoadDataFromRoad()
    {
        arrowMarkers.Clear();
        OnListCleared();
        IntegrityCheck();
    }
    public void IntegrityCheck()
    {
        if (GetMarkerCount() != arrowMarkers.Count)
        {
            for (int i = 0; i < GetMarkerCount(); i++)
            {
                AddMarkersToList(GetMarker(i) as ArrowMarker);
            }
        }
    }
    private void OnDestroy()
    {
       
    }
}
public enum RoadSide
{
    Player,
    Enemy
}