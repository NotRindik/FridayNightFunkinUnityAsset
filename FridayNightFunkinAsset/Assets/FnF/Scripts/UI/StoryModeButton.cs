using System;
using FnF.Scripts.Extensions;
using FridayNightFunkin.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using NullReferenceException = System.NullReferenceException;

namespace FnF.Scripts.UI
{
    public class StoryModeButton : ButtonBehaviour
    {
        private int Index => GetIndex();
        protected override void OnAnimationEnd()
        {
            var storyModeMenu = G.Instance.Get<StoryModeMenu>();
            try
            {
                if (storyModeMenu == null) throw new NullReferenceException("StoryModeMenu is null");
                if (storyModeMenu.levelConfigs == null) throw new NullReferenceException("levelConfigs is null");
                if (Index >= storyModeMenu.levelConfigs.Length) throw new IndexOutOfRangeException("Index out of range");
                if (storyModeMenu.levelConfigs[Index] == null) throw new NullReferenceException($"levelConfigs[{Index}] is null");
                if (storyModeMenu.levelConfigs[Index].levelScene == null) throw new NullReferenceException($"levelConfigs[{Index}].levelData is null");
                
                SceneManager.LoadScene(storyModeMenu.levelConfigs[Index].levelScene.name);
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
        
            Debug.Log("PathNotFound");
            return 0;
        }
    }
}