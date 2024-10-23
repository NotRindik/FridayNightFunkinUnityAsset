using System.ComponentModel;
using UnityEngine;
using UnityEngine.Timeline;

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
