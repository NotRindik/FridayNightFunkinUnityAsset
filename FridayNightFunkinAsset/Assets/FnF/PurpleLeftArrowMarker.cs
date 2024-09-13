using System.ComponentModel;
using UnityEngine;
using UnityEngine.Timeline;

[DisplayName("left")]
[CustomStyle("LeftArrow")]
public class PurpleLeftArrowMarker : ArrowMarker
{
    [SerializeField] protected ArrowSide arrowSide = ArrowSide.LeftArrow;
}
