using System.ComponentModel;
using UnityEngine;
using UnityEngine.Timeline;

[DisplayName("right")]
[CustomStyle("RightArrow")]
public class RedRightArrowMarker : ArrowMarker
{
    public override void OnInitialize(TrackAsset aPent)
    {
        arrowSide = ArrowSide.RightArrow;
        base.OnInitialize(aPent);
    }
}
