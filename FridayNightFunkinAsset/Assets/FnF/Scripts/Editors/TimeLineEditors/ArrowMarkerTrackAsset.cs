using FridayNightFunkin.Editor.TimeLineEditor;
using System.Collections.Generic;
#if UNITY_EDITOR
using FnF.Scripts.Editors;
#endif
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

[TrackColor(1f, 0.5f, 0f)]
[TrackBindingType(typeof(Null))]
[Icon("Assets/FnF/Editor/icon-bf.png")]
public class ArrowMarkerTrackAsset : MarkerTrack
{
    public RoadSide roadSide;
#if UNITY_EDITOR
    public LevelDataWindow levelDataWindow => EditorWindow.GetWindow<LevelDataWindow>();
#endif
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
    public void LoadDataFromRoad()
    {
        arrowMarkers.Clear();
        arrowCurrentIndex = 0;
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