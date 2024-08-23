using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[DisplayName("up")]
[CustomStyle("UpArrow")]
public class GreenUpArrowMarker : ArrowMarker
{
    [SerializeField] protected ArrowSide arrowSide = ArrowSide.UpArrow;
}
