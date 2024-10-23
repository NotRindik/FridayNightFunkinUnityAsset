using System.ComponentModel;
using UnityEngine;
using UnityEngine.Timeline;

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
