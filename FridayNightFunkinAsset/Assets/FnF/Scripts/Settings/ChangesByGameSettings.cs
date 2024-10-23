using UnityEngine;

namespace FridayNightFunkin.Settings
{
    public class ChangesByGameSettings : MonoBehaviour, IService
    {
        private RectTransform playerArrows;
        private RectTransform enemyArrows;

        public int downscroll { get; private set; }
        public int naughtyness { get; private set; }
        public int flashLights { get; private set; }
        public int cameraZoomingOnBeat { get; private set; }
        public int ghostTapping { get; private set; }
        public int autoPause { get; private set; }
        private ServiceLocator serviceLocator => ServiceLocator.instance;

        private void Awake()
        {
            GetSettingsValue();
            serviceLocator.Register(this);
        }
        private void OnDestroy()
        {
            serviceLocator.UnRegister<ChangesByGameSettings>();
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