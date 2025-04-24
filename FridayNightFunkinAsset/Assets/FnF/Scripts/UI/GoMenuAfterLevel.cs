using FnF.Scripts;
using FnF.Scripts.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace FridayNightFunkin.UI
{
    public class GoMenuAfterLevel : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnAfterLevelStoryMode;
        [SerializeField] private UnityEvent OnAfterLevelFreePlay;
        [SerializeField] private StoryModeMenu storyModeMenu;

        private void Start()
        {
            if (PlayerPrefs.GetInt("AfterLevel") == 1)
            {
                if (PlayerPrefs.GetInt(LevelSaveConst.IS_FROM_FREE_PLAY) == 0)
                {
                    storyModeMenu.Init();
                    EventSystem.current.SetSelectedGameObject(storyModeMenu.buttons[0].gameObject);
                    OnAfterLevelStoryMode?.Invoke();
                }
                else
                {
                    EventSystem.current.SetSelectedGameObject(G.Instance.Get<FreePlayMenu>().buttons[0].gameObject);
                    PlayerPrefs.SetInt(LevelSaveConst.IS_FROM_FREE_PLAY, 0);
                    OnAfterLevelFreePlay?.Invoke();
                }
                PlayerPrefs.SetInt("AfterLevel", 0);
            }
        }
    }
}