using System.ComponentModel;
using UnityEngine;
using UnityEngine.Timeline;

[DisplayName("right")]
[CustomStyle("RightArrow")]
public class RedRightArrowMarker : ArrowMarker
{
    [SerializeField] protected ArrowSide arrowSide = ArrowSide.RightArrow;
}
