using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[DisplayName("right")]
[CustomStyle("RightArrow")]
public class RedRightArrowMarker : ArrowMarker
{
    [SerializeField] protected ArrowSide arrowSide = ArrowSide.RightArrow;
}
