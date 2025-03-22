using System.Collections;
using System.Collections.Generic;
using FnF.Scripts.Extensions;
using FridayNightFunkin.Editor.TimeLineEditor;
using FridayNightFunkin.UI;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour,IService
{
    [SerializeField] public healthBarData healthBarData = new healthBarData();

    public void Init(ChartPlayBack chartPlayBack)
    {
        var bar = healthBarData.healthBar;
        bar.value = chartPlayBack.levelData.stage[ChartPlayBack.CurrentStageIndex].startHealth;
        bar.minValue = chartPlayBack.levelData.stage[ChartPlayBack.CurrentStageIndex].minHealth;
        bar.maxValue = chartPlayBack.levelData.stage[ChartPlayBack.CurrentStageIndex].maxHealth;
    }

    private void OnValidate()
    {
        if (!healthBarData.healthBar)
        {
            healthBarData.healthBar = GetComponent<Slider>();
        }
    }

    public void ModifyValue(float value)
    {
        ModifyProcess(healthBarData.healthBar.value + value, 1f);
    }

    private void ModifyProcess(float targetValue, float initialAdder)
    {
        if (healthBarData.SliderMoveProcess != null)
        {
            StopCoroutine(healthBarData.SliderMoveProcess);
        }

        healthBarData.SliderMoveProcess = StartCoroutine(ModifyProcessCO(targetValue, initialAdder));
    }

    private IEnumerator ModifyProcessCO(float targetValue, float initialAdder)
    {
        while (true)
        {
            yield return null;

            healthBarData.healthBar.value = Mathf.MoveTowards(healthBarData.healthBar.value, targetValue, initialAdder);

            if (healthBarData.healthBar.value == targetValue)
            {
                break;
            }
        }
    }
}

[System.Serializable]
public class healthBarData
{
    public Slider healthBar;
    public Coroutine SliderMoveProcess;
    public List<IconInSlider> iconsInSlider;
}
