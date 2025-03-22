using System;
using FridayNightFunkin.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace FnF.Scripts.UI
{
    public class FreePlayButton : ButtonBehaviour
    {
        private int Index => GetIndex();

        public LevelData LevelData;
        public int stageIndex;
    
        public void Init( LevelData levelData,int stage)
        {
            LevelData = levelData;
            stageIndex = stage;
        }
        public void OnStart()
        {
            try
            {
                if (LevelData == null) throw new NullReferenceException($"levelConfigs[{Index}].levelData is null");
                LevelManager.CurrentLevelData = LevelData;
                PlayerPrefs.SetInt(LevelManager.STAGE_PLAYERPREFS_NAME,stageIndex);
                PlayerPrefs.SetInt(LevelManager.IS_FROM_FREE_PLAY,1);
                SceneManager.LoadScene("LevelsScene");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error in OnAnimationEnd: {e.Message}\n{e.StackTrace}");
                throw;
            }
        }

        private int GetIndex()
        {
            var parent = transform.parent;
            for (int i = 0; i < parent.childCount; i++)
            {
                if (parent.GetChild(i) == this.transform)
                {
                    return i;
                }
            }
            return 0;
        }
    }
}
