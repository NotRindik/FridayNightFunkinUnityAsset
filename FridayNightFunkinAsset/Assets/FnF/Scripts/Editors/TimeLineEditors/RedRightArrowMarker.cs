using System.ComponentModel;
using UnityEngine.Timeline;
namespace FridayNightFunkin.Editor.TimeLineEditor
{
    [DisplayName("Add Right")]
    [CustomStyle("RightArrow")]
    public class RedRightArrowMarker : ArrowMarker
    {
        public override void OnInitialize(TrackAsset aPent)
        {
            arrowSide = ArrowSide.RightArrow;
            base.OnInitialize(aPent);
        }
    }
}