using System.ComponentModel;
using UnityEngine.Timeline;

namespace FridayNightFunkin.Editor.TimeLineEditor
{
    [DisplayName("down")]
    [CustomStyle("DownArrow")]
    public class BlueDownArrowMarker : ArrowMarker
    {
        public override void OnInitialize(TrackAsset aPent)
        {
            arrowSide = ArrowSide.DownArrow;
            base.OnInitialize(aPent);
        }
    }
}