using FridayNightFunkin.Editor;
using FridayNightFunkin.Editor.TimeLineEditor;
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

    private void OnDisable()
    {
        ArrowMarkerManager.instance.OnListCleared -= OnListCleared;
    }

    private void OnEnable()
    {
        foreach (ArrowMarker item in GetMarkers())
        {
            item.ArrowInit(this);
        }
        ArrowMarkerManager.instance.OnListCleared += OnListCleared;
    }

    private void OnListCleared()
    {
        IsActive();
        if (!isActive)
            return;
        ArrowMarkerManager.instance.saveRoad[(int)roadSide] = null;
        ArrowMarkerManager.instance.IntegrityCheck(this);
    }
    private void OnDestroy()
    {
        ArrowMarkerManager.instance.OnListCleared -= OnListCleared;
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