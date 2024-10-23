using UnityEngine;

namespace FridayNightFunkin.UI
{
    public class AutoPauseSetting : ToggleBehaviour
    {
        protected override void OnToggleTriggered(bool isTrue)
        {
            base.OnToggleTriggered(isTrue);
            Application.runInBackground = !isTrue;
        }
    }
}