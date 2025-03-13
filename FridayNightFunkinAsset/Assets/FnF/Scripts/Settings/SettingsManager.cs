using System;
using FnF.Scripts.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace FnF.Scripts.Settings
{
    [ExecuteAlways]
    public class SettingsManager : MonoBehaviour,IService
    {
        public GameSettingsSO activeGameSettings;
        public string savePath;

        public bool manualSave;
        public bool manualLoad;
        public void Init()
        {
            Load();
        }

        private void Update()
        {
            if (manualSave)
            {
                Save();
                manualSave = false;
            }
            
            if (manualLoad)
            {
                Load();
                manualLoad = false;
            }
        }

        public void Save()
        {
            SaveLoadSystem.SaveSettings(activeGameSettings,savePath);
        }
        
        public void Load()
        {
            SaveLoadSystem.LoadSettings(activeGameSettings,savePath);
        }
    }
}
