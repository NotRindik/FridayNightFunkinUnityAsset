using System;
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
        [SerializeField] private RoadSide roadSide;
        private ChartPlayBack chartPlayBack;
        private void Start()
        {
            image = GetComponent<Image>();
            var healthBarData = G.Instance.Get<HealthBar>().healthBarData;
            var healthBar = healthBarData.healthBar;
            healthBar.onValueChanged.AddListener(IconsChanging);
            healthBarData.iconsInSlider.Add(this);
            chartPlayBack = G.Instance.Get<ChartPlayBack>();
            transform.parent.TryGetComponent(out PlayAnimPerBeat animator);
            animator.ChangeBPM(chartPlayBack.levelData.stage[chartPlayBack.levelData.selectedStageIndex].BPM);
            
            IconsChanging(healthBar.value);
        }

        private void IconsChanging(float value)
        {
            float percent = (value + 100) / 200f * 100f;

            var stage = chartPlayBack.levelData.stage[ChartPlayBack.CurrentStageIndex];

            if (roadSide == RoadSide.Player)
            {
                if (percent < 10)
                {
                    image.sprite = stage.playerIcon[IconProgressStatus.Losing];
                }
                else if (percent is >= 10 and <= 90)
                {
                    image.sprite = stage.playerIcon[IconProgressStatus.Mid];
                }
                else
                {
                    if (stage.playerIcon[IconProgressStatus.Winning] != null)
                    {
                        image.sprite = stage.playerIcon[IconProgressStatus.Winning];
                    }
                }
            }
            else
            {
                if (percent > 90) 
                {
                    image.sprite = stage.enemyIcon[IconProgressStatus.Losing];
                }
                else if (percent is >= 10 and <= 90)
                {
                    image.sprite = stage.enemyIcon[IconProgressStatus.Mid];
                }
                else
                {
                    if (stage.playerIcon[IconProgressStatus.Winning] != null)
                    {
                        image.sprite = stage.enemyIcon[IconProgressStatus.Winning];
                    }
                }
            }
        }
    }
}