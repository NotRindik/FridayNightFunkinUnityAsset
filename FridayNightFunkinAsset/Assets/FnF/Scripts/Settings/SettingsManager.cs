using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace FnF.Scripts.Settings
{
    [ExecuteAlways]
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager Instance { get; private set; }

        public GameSettingsSO activeGameSettings;
        public string savePath;

        public bool manualSave;
        public bool manualLoad;
        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
            {
                Destroy(this);
            }

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
