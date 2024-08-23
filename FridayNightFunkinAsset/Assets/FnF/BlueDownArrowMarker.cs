using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[DisplayName("down")]
[CustomStyle("DownArrow")]
public class BlueDownArrowMarker : ArrowMarker
{
    [SerializeField] protected ArrowSide arrowSide = ArrowSide.DownArrow;
}
