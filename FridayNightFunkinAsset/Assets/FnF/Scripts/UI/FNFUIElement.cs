using UnityEngine;
using TMPro;
using UnityEngine.UI;
using FridayNightFunkin.Calculations;
using FridayNightFunkin.Settings;

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
        }

        private void Start()
        {
            if (ChangesByGameSettings.instance.downscroll == 1)
            {
                ChangeAnchorToTop(hud.rectTransform);
                hud.rectTransform.anchoredPosition = new Vector2(hud.rectTransform.anchoredPosition.x, -15); //Top

                var slider = versusSlider.transform.parent.GetComponent<RectTransform>();
                ChangeAnchorToTop(slider);
                slider.anchoredPosition = new Vector2(hud.rectTransform.anchoredPosition.x, -50); //Top
            }
            UpdateUI();
        }

        private void ChangeAnchorToTop(RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(0.5f, 1);
            rectTransform.anchorMax = new Vector2(0.5f, 1);
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