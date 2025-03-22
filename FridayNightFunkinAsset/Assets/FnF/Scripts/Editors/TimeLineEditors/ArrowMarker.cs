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

        public int id;
        public RoadSide roadSide = RoadSide.Player;

        public ArrowSide arrowSide;
        protected PropertyName objectID => new PropertyName();
        public ArrowMarkerTrackAsset arrowMarkerParent { private set; get; }

        public float speedMultiplier = 1;

        private double currentTime;

        public bool isInit;
        

        public override async void OnInitialize(TrackAsset aPent)
        {
            isInit = false;
            _ = InitializeWithDelay(aPent);
        }
        
        private async Task InitializeWithDelay(TrackAsset aPent)
        {
            await Task.Delay(100); // Ждём 100 мс

            if (!isInit && aPent is ArrowMarkerTrackAsset trackAsset)
            {
                arrowMarkerParent = trackAsset;
                id = trackAsset.arrowCurrentIndex;
                roadSide = trackAsset.roadSide;
                trackAsset.AddMarkersToList(this);
            }
            isInit = true;
        }
        public void ArrowInit(ArrowMarkerTrackAsset arrowMarkerParent)
        {
            this.arrowMarkerParent = arrowMarkerParent;
            roadSide = arrowMarkerParent.roadSide;
        }
        protected void OnDestroy()
        {
            if (parent)
            {
                ((ArrowMarkerTrackAsset)parent).RemoveMarker(this);
#if UNITY_EDITOR
                Undo.ClearUndo(this);
#endif
            }
        }
    }
}