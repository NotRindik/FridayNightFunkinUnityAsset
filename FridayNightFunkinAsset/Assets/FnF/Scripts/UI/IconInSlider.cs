using FridayNightFunkin.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace FridayNightFunkin.UI
{
    public class IconInSlider : MonoBehaviour
    {
        private Image image;
        public bool isPlayer = false;

        private void Start()
        {
            image = GetComponent<Image>();
            FNFUIElement.instance.versusSlider.onValueChanged.AddListener(IconsChanging);
            IconsChanging(FNFUIElement.instance.versusSlider.value);
        }

        private void IconsChanging(float value)
        {
            if (isPlayer)
            {
                if (value < -60)
                {
                    image.sprite = LevelSettings.instance.stage[LevelSettings.instance.stageIndex].playerIcon[IconProgressStatus.Losing];
                }
                else
                {
                    image.sprite = LevelSettings.instance.stage[LevelSettings.instance.stageIndex].playerIcon[IconProgressStatus.Mid];
                }
            }
            else
            {
                if (value > 60)
                {
                    image.sprite = LevelSettings.instance.stage[LevelSettings.instance.stageIndex].enemyIcon[IconProgressStatus.Mid];
                }
                else
                {
                    image.sprite = LevelSettings.instance.stage[LevelSettings.instance.stageIndex].enemyIcon[IconProgressStatus.Losing];
                }
            }
        }
    }
}