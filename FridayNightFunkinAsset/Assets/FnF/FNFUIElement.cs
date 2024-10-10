using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace FridayNightFunkin.UI
{
    public class FNFUIElement : MonoBehaviour
    {
        [Header("UIElements")]
        [SerializeField] private TextMeshProUGUI hud;

        [SerializeField] public Slider versusSlider;

        public static FNFUIElement instance { get; private set; }

        private ScoreManager scoreManager => ScoreManager.instance;

        private void Awake()
        {
            if (!instance)
            {
                instance = this;
            }
            else
            {
                DestroyImmediate(this.gameObject);
            }
            UpdateUI();
        }

        public void UpdateUI()
        {
            if (scoreManager.accuracyList.Count == 0)
            {
                hud.text = $"misses:{scoreManager.misses}|Combo:{scoreManager.combo}|Score:{scoreManager.score}|Rating:?)";
            }
            else
                hud.text = $"misses:{scoreManager.misses}|Combo:{scoreManager.combo}|Score:{scoreManager.score}|Rating:{scoreManager.GetRatingByAccuracy(scoreManager.totalAccuracy)}({scoreManager.totalAccuracy}%)";
        }
    }
}