using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using UnityEngine.Timeline;
using FridayNightFunkin.GamePlay;

namespace FridayNightFunkin.Editor.TimeLineEditor
{
    [HideInMenu]

    public class ArrowMarker : Marker
    {
        [SerializeField] public uint distanceCount;
        private uint _distanceCount;

        public int id;
        public RoadSide roadSide = RoadSide.Player;

        public ArrowSide arrowSide;
        protected PropertyName objectID => new PropertyName();
        public ArrowMarkerTrackAsset arrowMarkerParent { private set; get; }

        public float speedMultiplier = 1;
        private float _speedMultiplier;

        private double currentTime;

        public bool isInit;

        public Arrow arrow;

        public override async void OnInitialize(TrackAsset aPent)
        {
            EditorApplication.update += Update;
            isInit = false;
            await Task.Delay(100);
            
            if (!isInit)
            {
                if (aPent is ArrowMarkerTrackAsset)
                {
                    arrowMarkerParent = aPent as ArrowMarkerTrackAsset;
                    id = arrowMarkerParent.arrowCurrentIndex;
                    roadSide = arrowMarkerParent.roadSide;
                    _speedMultiplier = speedMultiplier;
                    _distanceCount = distanceCount;
                    arrowMarkerParent.AddMarkersToList(this);
                }
                isInit = true;
            }
        }

        private void Update()
        {
            if (arrow)
            {
                if (_speedMultiplier != speedMultiplier || _distanceCount != distanceCount || Math.Abs(currentTime - time) > double.Epsilon)
                {
                    _speedMultiplier = speedMultiplier;
                    _distanceCount = distanceCount;
                    currentTime = time;
                    arrow.OnParamChanged(currentTime, Mathf.Abs(_speedMultiplier) != 0 ? Mathf.Abs(_speedMultiplier) : 0.0001f, _distanceCount);
                }
            }
        }
        public void ArrowInit(ArrowMarkerTrackAsset arrowMarkerParent)
        {
            this.arrowMarkerParent = arrowMarkerParent;
            roadSide = arrowMarkerParent.roadSide;
        }
        protected void OnDestroy()
        {
            if (parent && arrow)
            {
                ((ArrowMarkerTrackAsset)parent).RemoveMarker(this);
                EditorApplication.update -= Update;   
                Undo.ClearUndo(this);
            }
        }
    }
}