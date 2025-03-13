using System;
using FnF.Scripts.Extensions;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using FridayNightFunkin.Calculations;
using FridayNightFunkin.Settings;

namespace FridayNightFunkin.UI
{
    public class FNFStatisticDisplay : MonoBehaviour,IService
    {
        [Header("UIElements")]
        [SerializeField] private TextMeshProUGUI hud;

        private StatisticManager _statisticManager;

        public void Init(StatisticManager statisticManager)
        {
            _statisticManager = statisticManager;
            UpdateUI();
            _statisticManager.OnValueChanged += UpdateUI;
        }
        private void OnDisable()
        {
            _statisticManager.OnValueChanged -= UpdateUI;
        }

        public void UpdateUI()
        {
            if (_statisticManager.accuracyList.Count == 0)
            {
                hud.text = $"misses:{_statisticManager.misses}|Combo:{_statisticManager.combo}|Score:{_statisticManager.score}|Rating:?)";
            }
            else
                hud.text = $"misses:{_statisticManager.misses}|Combo:{_statisticManager.combo}|Score:{_statisticManager.score}|Rating:{_statisticManager.GetRatingByAccuracy(_statisticManager.totalAccuracy)}({_statisticManager.totalAccuracy}%)";
        }
    }
}