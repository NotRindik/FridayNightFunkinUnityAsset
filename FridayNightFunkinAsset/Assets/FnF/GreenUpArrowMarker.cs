using System.ComponentModel;
using UnityEngine;
using UnityEngine.Timeline;

[DisplayName("up")]
[CustomStyle("UpArrow")]
public class GreenUpArrowMarker : ArrowMarker
{
    [SerializeField] protected ArrowSide arrowSide = ArrowSide.UpArrow;
}
