using System.ComponentModel;
using UnityEngine;
using UnityEngine.Timeline;

[DisplayName("down")]
[CustomStyle("DownArrow")]
public class BlueDownArrowMarker : ArrowMarker
{
    [SerializeField] protected ArrowSide arrowSide = ArrowSide.DownArrow;
}
