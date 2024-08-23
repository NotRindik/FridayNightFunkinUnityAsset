using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[DisplayName("left")]
[CustomStyle("LeftArrow")]
public class PurpleLeftArrowMarker : ArrowMarker
{
    [SerializeField] protected ArrowSide arrowSide = ArrowSide.LeftArrow;
}
