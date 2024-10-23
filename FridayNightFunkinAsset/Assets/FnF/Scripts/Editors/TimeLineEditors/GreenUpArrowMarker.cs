using System.ComponentModel;
using UnityEngine.Timeline;

namespace FridayNightFunkin.Editor.TimeLineEditor
{
    [DisplayName("up")]
    [CustomStyle("UpArrow")]
    public class GreenUpArrowMarker : ArrowMarker
    {
        public override void OnInitialize(TrackAsset aPent)
        {
            arrowSide = ArrowSide.UpArrow;
            base.OnInitialize(aPent);
        }
    }
}