using FridayNightFunkin.Calculations;
using UnityEngine;

namespace FridayNightFunkin.UI
{
    public class AccuracyCombo : MonoBehaviour
    {
        public Sprite[] comboSpites;

        private ParticleSystem _particleSystem;

        private int curentIndex;

        private void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            ScoreManager.instance.OnCalculateAccuracy += SpawnCombo;
        }

        private void OnDisable()
        {
            ScoreManager.instance.OnCalculateAccuracy -= SpawnCombo;
        }
        private void OnDestroy()
        {
            ScoreManager.instance.OnCalculateAccuracy -= SpawnCombo;
        }

        private void SpawnCombo(int accuracy)
        {
            if (curentIndex != ScoreManager.instance.GetRatingByAccuracyInt(accuracy))
            {
                curentIndex = ScoreManager.instance.GetRatingByAccuracyInt(accuracy);
                if (curentIndex != -1)
                {
                    _particleSystem.textureSheetAnimation.SetSprite(0, comboSpites[curentIndex]);
                }
            }
            if (curentIndex != -1) _particleSystem.Emit(1);

        }
    }
}