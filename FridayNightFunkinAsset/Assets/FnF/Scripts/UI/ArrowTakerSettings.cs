using FnF.Scripts.Settings;
using FridayNightFunkin.UI;
using UnityEngine;

public class ArrowTakerSettings : MonoBehaviour
{
    private RectTransform rectTransform;
    ArrowMask arrowMask => ArrowMask.instance;

    public void Init(SettingsManager settingsManager)
    {
        rectTransform = GetComponent<RectTransform>();
        if (settingsManager.activeGameSettings.Downscroll == 1)
        {
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -500); //Top
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, 0); //Bottom
        }
        else
        {
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, 0); //Top
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, 500); //Bottom
        }
    }
}
