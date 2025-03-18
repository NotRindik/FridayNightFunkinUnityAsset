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
            return (int)GetType().GetField(settingName).GetValue(this) != 0;
        }
        public void SetSettingValue(bool value,string settingName)
        {
            int val = value == true ? 1 : 0;
            GetType().GetField(settingName).SetValue(this, val);
        }
    }
}