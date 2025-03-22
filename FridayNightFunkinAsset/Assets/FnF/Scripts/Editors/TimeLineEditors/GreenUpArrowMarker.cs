using System.ComponentModel;
using FridayNightFunkin.Editor.TimeLineEditor;
using UnityEngine.Timeline;

namespace FnF.Editors.TimeLineEditors
{
    [DisplayName("Add Up")]
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