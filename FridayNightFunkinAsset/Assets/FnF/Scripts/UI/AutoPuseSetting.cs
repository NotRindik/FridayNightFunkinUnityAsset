using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class AutoPuseSetting : ToggleBehaviour
{
    protected override void OnToggleTriggered(bool isTrue)
    {
        base.OnToggleTriggered(isTrue);
        Application.runInBackground = !isTrue;
    }
}
