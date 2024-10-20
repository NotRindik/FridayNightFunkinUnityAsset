using UnityEngine;
using UnityEngine.UI;

public class ToggleBehaviour : MonoBehaviour
{
    [Header("All settings are saved in playprefs with the toggle object NAME in the inspector.")]
    private Toggle toggle;

    private void OnEnable()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleTriggered);
        toggle.isOn = PlayerPrefs.GetInt($"{gameObject.name}") == 1 ? true : false;
    }

    public void OnToggleTriggered(bool isTrue)
    {
        PlayerPrefs.SetInt($"{gameObject.name}", isTrue == true ? 1 : 0);
    }

    private void OnDisable()
    {
        toggle.onValueChanged.RemoveListener(OnToggleTriggered);
    }
}
