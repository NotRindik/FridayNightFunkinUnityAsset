using UnityEngine;

namespace FridayNightFunkin.Settings
{
    public class ChangesByGameSettings : MonoBehaviour
    {
        private RectTransform playerArrows;
        private RectTransform enemyArrows;

        public int downscroll { get; private set; }
        public int naughtyness { get; private set; }
        public int flashLights { get; private set; }
        public int cameraZoomingOnBeat { get; private set; }
        public int ghostTapping { get; private set; }
        public int autoPause { get; private set; }

        public static ChangesByGameSettings instance { get; private set; }

        private void Awake()
        {
            GetSettingsValue();
            if(instance == null )
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
        //All settings are saved in playprefs with the toggle object NAME in the inspector.
        private void GetSettingsValue()
        {
            naughtyness = PlayerPrefs.GetInt("Naughtyness");
            downscroll = PlayerPrefs.GetInt("DownScroll");
            flashLights = PlayerPrefs.GetInt("FlashLights");
            ghostTapping = PlayerPrefs.GetInt("GhostTapping");
            cameraZoomingOnBeat = PlayerPrefs.GetInt("Camera Zooming On Beat");
            autoPause = PlayerPrefs.GetInt("AutoPause");
        }
    }
}