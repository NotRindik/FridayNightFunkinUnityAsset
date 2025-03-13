using FnF.Scripts.Extensions;
using FnF.Scripts.Settings;
using FridayNightFunkin;
using FridayNightFunkin.Calculations;
using FridayNightFunkin.Editor.TimeLineEditor;
using FridayNightFunkin.GamePlay;
using FridayNightFunkin.UI;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace FnF.Scripts
{
    public class BootstrapGameScene : MonoBehaviour,IBootstrap
    {
        public ChartPlayBack chartPlayBack;
        public HealthBar healthBar;
        public FNFStatisticDisplay statDisplay;
        public StatisticManager statManager;
        public AccuracyCombo accuracyCombo;
        public CharacterSpawner characterSpawner;
        public SettingsManager settingsManager;
        public PlayerDeath playerDeath;
        public G g;
        public MapSpawner mapSpawner;
        public SceneLoad sceneLoad;

        public CharacterCamera[] characterCameras;

        protected void OnValidate()
        {
            if(!g)
                g = FindObjectOfType<G>();
        }

        public void Awake()
        {
            Initialize();
            RegisterServices();
        }
        private void Initialize()
        {
            g.Initialize(); //Service Locator Init
            
            statDisplay.Init(statManager);
            accuracyCombo.Init(statManager);
            settingsManager.Init();
            
            chartPlayBack.InitOnGameMode(settingsManager);
            healthBar.Init(chartPlayBack);
            
            mapSpawner.OnSpawnMapEnd += InitAfterMapSpawn;
            mapSpawner.Init(chartPlayBack);
        }

        private void InitAfterMapSpawn()
        {
            characterSpawner.Init(chartPlayBack,mapSpawner);
            playerDeath.Init();
            foreach (var cam in characterCameras)
            {
                cam.Init(mapSpawner);   
            }
            
            if (chartPlayBack.playOnStart)
            {
                chartPlayBack.StartLevel();
            }   
        }
        private void RegisterServices()
        {
            g.Register(healthBar);
            g.Register(chartPlayBack);
            g.Register(statDisplay);
            g.Register(statManager);
            g.Register(accuracyCombo);
            g.Register(settingsManager);
            g.Register(playerDeath);
            g.Register(characterSpawner);
            g.Register(mapSpawner);
            g.Register(sceneLoad);
        }
    }
}
