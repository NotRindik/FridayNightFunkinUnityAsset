using System.ComponentModel;
using FridayNightFunkin.Editor.TimeLineEditor;
using UnityEngine.Timeline;

namespace FnF.Editors.TimeLineEditors
{
    [DisplayName("up/Add up")]
    [CustomStyle("UpArrow")]
    public class GreenUpArrowMarker : ArrowMarker
    {
        public override void OnInitialize(TrackAsset aPent)
        {
            arrowSide = ArrowSide.UpArrow;
            base.OnInitialize(aPent);
        }
    }
}