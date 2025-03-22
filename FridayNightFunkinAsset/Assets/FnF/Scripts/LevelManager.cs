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
    }
    
    
    public static class ScoreManager
    {
        public const string STAGE_PERSONAL_RECORD_PREFIX = ".personal_record";
    }
}
