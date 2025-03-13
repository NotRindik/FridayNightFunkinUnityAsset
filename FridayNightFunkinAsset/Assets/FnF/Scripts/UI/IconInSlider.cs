using FnF.Scripts.Extensions;
using FridayNightFunkin.Editor.TimeLineEditor;
using FridayNightFunkin.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace FridayNightFunkin.UI
{
    public class IconInSlider : MonoBehaviour
    {
        private Image image;
        public bool isPlayer = false;
        public ChartPlayBack chartPlayBack;

        private void OnValidate()
        {
            if(chartPlayBack == null)
            {
                chartPlayBack = FindAnyObjectByType<ChartPlayBack>();
            }
        }

        private void Start()
        {
            image = GetComponent<Image>();
            var healthBar = G.Instance.Get<HealthBar>().healthBarData.healthBar;
            healthBar.onValueChanged.AddListener(IconsChanging);
            IconsChanging(healthBar.value);
        }

        private void IconsChanging(float value)
        {
            if (isPlayer)
            {
                if (value < -60)
                {
                    image.sprite = chartPlayBack.levelData.stage[chartPlayBack.currentStageIndex].playerIcon[IconProgressStatus.Losing];
                }
                else
                {
                    image.sprite = chartPlayBack.levelData.stage[chartPlayBack.currentStageIndex].playerIcon[IconProgressStatus.Mid];
                }
            }
            else
            {
                if (value > 60)
                {
                    image.sprite = chartPlayBack.levelData.stage[chartPlayBack.currentStageIndex].enemyIcon[IconProgressStatus.Mid];
                }
                else
                {
                    image.sprite = chartPlayBack.levelData.stage[chartPlayBack.currentStageIndex].enemyIcon[IconProgressStatus.Losing];
                }
            }
        }
    }
}