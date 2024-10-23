using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace FridayNightFunkin.UI
{
    public class GoMenuAfterLevel : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnAfterLevel;
        [SerializeField] private StoryModeMenu storyModeMenu;

        private void Start()
        {
            if (PlayerPrefs.GetInt("AfterLevel") == 1)
            {
                storyModeMenu.Initialize();
                EventSystem.current.SetSelectedGameObject(storyModeMenu.buttons[0].gameObject);
                PlayerPrefs.SetInt("AfterLevel", 0);
                OnAfterLevel?.Invoke();
            }
        }
    }
}