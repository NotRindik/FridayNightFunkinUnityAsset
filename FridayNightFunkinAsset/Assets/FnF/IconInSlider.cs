using FridayNightFunkin;
using FridayNightFunkin.UI;
using UnityEngine;
using UnityEngine.UI;

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
                image.sprite = LevelSettings.instance.stage[LevelSettings.instance.stageIndex].playerIcon[1];
            }
            else
            {
                image.sprite = LevelSettings.instance.stage[LevelSettings.instance.stageIndex].playerIcon[0];
            }
        }
        else
        {
            if (value > 60)
            {
                image.sprite = LevelSettings.instance.stage[LevelSettings.instance.stageIndex].enemyIcon[1];
            }
            else
            {
                image.sprite = LevelSettings.instance.stage[LevelSettings.instance.stageIndex].enemyIcon[0];
            }
        }
    }
}
