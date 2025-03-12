using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] healthBarData healthBarData = new healthBarData();

    private void OnValidate()
    {
        if (!healthBarData.healthBar)
        {
            healthBarData.healthBar = GetComponent<Slider>();
        }
    }

    public void AddValueToSlider(float value)
    {
        StartMoveSliderSmoothly(healthBarData.healthBar.value + value, 1f);
    }

    public void StartMoveSliderSmoothly(float targetValue, float initialAdder)
    {
        if (healthBarData.sliderMoveProcess != null)
        {
            StopCoroutine(healthBarData.sliderMoveProcess);
        }

        healthBarData.sliderMoveProcess = StartCoroutine(MoveSliderSmoothlyCoroutine(targetValue, initialAdder));
    }

    public IEnumerator MoveSliderSmoothlyCoroutine(float targetValue, float initialAdder)
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
    public Coroutine sliderMoveProcess;
}
