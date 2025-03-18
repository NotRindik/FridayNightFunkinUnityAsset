using System.Collections.Generic;
using UnityEngine;

namespace FnF.Scripts
{
    public static class LevelManager
    {
        private static LevelData _currentLevelData;
        public const string STAGE_PLAYERPREFS_NAME = "CurrentStage";
        public const string DIFFICULTY_PLAYERPREFS_NAME = "Difficult";
        public const string IS_FROM_FREE_PLAY = "FreePlay";
        public static LevelData CurrentLevelData
        {
            get
            {
                if (_currentLevelData == null)
                {
                    Debug.LogError("You Didnt set Level Data, pls set it");
                }

                return _currentLevelData;
            }
            set
            {
                _currentLevelData = value;
            }
        }
        
        public static Dictionary<string, LevelData> levels = new Dictionary<string, LevelData>();

        public static void RegisterLevel(string key, LevelData levelData)
        {
            if (!levels.ContainsKey(key))
            {
                levels[key] = levelData;
            }
        }

        public static LevelData GetLevel(string key)
        {
            if (levels.TryGetValue(key, out LevelData level))
            {
                return level;
            }
            Debug.LogError($"Level '{key}' not found!");
            return null;
        }
    }
    
    
    public static class ScoreManager
    {
        public const string STAGE_PERSONAL_RECORD_PREFIX = ".personal_record";
    }
}
