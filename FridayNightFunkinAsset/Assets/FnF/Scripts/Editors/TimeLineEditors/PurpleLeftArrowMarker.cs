using System.ComponentModel;
using UnityEngine.Timeline;

namespace FridayNightFunkin.Editor.TimeLineEditor
{
    [DisplayName("left")]
    [CustomStyle("LeftArrow")]
    public class PurpleLeftArrowMarker : ArrowMarker
    {
        public override void OnInitialize(TrackAsset aPent)
        {
            arrowSide = ArrowSide.LeftArrow;
            base.OnInitialize(aPent);
        }
    }
}