using UnityEngine;
using UnityEngine.UI;

public class ToggleBehaviour : MonoBehaviour
{
    [Header("All settings are saved in playprefs with the toggle object NAME in the inspector.")]
    protected Toggle toggle;
    [SerializeField] protected bool isFirstTimeTrue;

    protected void OnEnable()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleTriggered);
        if (!isFirstTimeTrue)
        {
            toggle.isOn = PlayerPrefs.GetInt($"{gameObject.name}") == 1 ? true : false;
        }
        else
        {
            toggle.isOn = true;
            PlayerPrefs.SetInt($"{gameObject.name}",1);
        }
    }

    protected virtual void OnToggleTriggered(bool isTrue)
    {
        PlayerPrefs.SetInt($"{gameObject.name}", isTrue == true ? 1 : 0);
    }

    private void OnDisable()
    {
        toggle.onValueChanged.RemoveListener(OnToggleTriggered);
    }
}
