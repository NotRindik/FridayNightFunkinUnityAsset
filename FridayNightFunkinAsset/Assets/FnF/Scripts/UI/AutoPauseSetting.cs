using UnityEngine;

namespace FridayNightFunkin.UI
{
    public class AutoPauseSetting : ToggleBehaviour
    {
        protected override void OnToggleTriggered(bool value)
        {
            base.OnToggleTriggered(value);
            Application.runInBackground = !value;
        }
    }
}