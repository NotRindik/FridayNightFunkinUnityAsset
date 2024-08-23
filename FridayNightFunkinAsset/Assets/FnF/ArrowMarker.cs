using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[CustomStyle("Arrow")] 
public class ArrowMarker : Marker, INotification, INotificationOptionProvider
{
    [SerializeField] protected bool emitOnce;
    [SerializeField] protected bool emitInEditor;
    [SerializeField] public uint distanceCount;

    public PropertyName id => new PropertyName();

    NotificationFlags INotificationOptionProvider.flags =>
       (emitOnce ? NotificationFlags.TriggerOnce : default) |
       (emitInEditor ? NotificationFlags.TriggerInEditMode : default);
}
