using System.IO;
using UnityEngine;

namespace FnF.Scripts.Settings
{
    public static class SaveLoadSystem
    {
        public static void SaveSettings(GameSettingsSO settings, string path)
        {
            string json = JsonUtility.ToJson(settings, true); // true = pretty print
            var fullPath = $"{Application.persistentDataPath}/{path}/config.json";
            string directoryPath = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            File.WriteAllText(fullPath, json);
            Debug.Log("Settings saved to: " + fullPath);
        }

        public static void LoadSettings(GameSettingsSO settings, string path)
        {
            var fullPath = $"{Application.persistentDataPath}/{path}/config.json";
            if (File.Exists(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                
                JsonUtility.FromJsonOverwrite(json, settings);

                Debug.Log("Settings loaded from: " + fullPath);
            }
            else
            {
                Debug.LogWarning("Settings file not found at: " + fullPath);
            }
        }
    }
}