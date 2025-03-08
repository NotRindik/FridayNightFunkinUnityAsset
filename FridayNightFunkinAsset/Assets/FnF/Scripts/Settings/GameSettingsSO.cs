using UnityEngine;
namespace FnF.Scripts.Settings
{
    [CreateAssetMenu(menuName = "Settings/GameSettings")]
    public class GameSettingsSO : ScriptableObject
    {
        public int Downscroll;
        public int Naughtyness;
        public int FlashLights;
        public int CameraZoomingOnBeat ;
        public int GhostTapping;
        public int AutoPause;
        
        
        public bool GetSettingValue(string settingName)
        {
            return (bool)this.GetType().GetField(settingName).GetValue(this);
        }
        public void SetSettingValue(bool value,string settingName)
        {
            this.GetType().GetField(settingName).SetValue(this, value);
        }
    }
}