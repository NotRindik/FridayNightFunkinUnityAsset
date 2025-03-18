using FnF.Scripts.Extensions;
using FnF.Scripts.Settings;
using FridayNightFunkin.GamePlay;
using FridayNightFunkin.UI;
using UnityEngine;
namespace FnF.Scripts
{
    public class BootstrapMainMenuScene : MonoBehaviour,IBootstrap
    {
        public G g;
        public SettingsManager settingsManager;
        public StoryModeMenu storyModeMenu;
        public FreePlayMenu freePlay;
        private void Awake()
        {
            Initialize();
            Register();
        }
        private void Initialize()
        {
            g.Init();
            storyModeMenu.Init();
            settingsManager.Init();
            freePlay.Init();
            InputManager.InputActions = new FnfInput();
        }

        private void Register()
        {
            g.Register(settingsManager);
            g.Register(storyModeMenu);
            g.Register(freePlay);
        }
        
    }
}
