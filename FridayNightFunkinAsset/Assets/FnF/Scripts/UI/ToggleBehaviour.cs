using System;
using FnF.Scripts.Settings;
using FridayNightFunkin.GamePlay;
using UnityEngine;
using UnityEngine.UI;

namespace FridayNightFunkin.UI
{
    public class ToggleBehaviour : MonoBehaviour
    {
        protected Toggle toggle;
        [SerializeField] protected bool isFirstTimeTrue;
        private GameSettingsSO _gameSettingsSo;
        private GameSettingsSO GameSettingsSo
        {
            get
            {
                if (!_gameSettingsSo)
                {
                    _gameSettingsSo = SettingsManager.Instance.activeGameSettings;
                }
                return _gameSettingsSo;
            }
        }
        [SerializeField] private string settingName;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(settingName))
            {
                settingName = gameObject.name.Replace(" ", "");
            }
        }

        protected void OnEnable()
        {
            toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(OnToggleTriggered);
            toggle.isOn = GameSettingsSo.GetSettingValue(settingName);
        }


        protected virtual void OnToggleTriggered(bool value)
        {
            GameSettingsSo.SetSettingValue(value,settingName);
            SettingsManager.Instance.Save();
        }

        private void OnDisable()
        {
            toggle.onValueChanged.RemoveListener(OnToggleTriggered);
        }
    }
}