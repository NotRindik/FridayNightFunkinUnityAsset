using FnF.Scripts.Extensions;
using FridayNightFunkin.Calculations;
using UnityEngine;

namespace FridayNightFunkin.UI
{
    public class AccuracyCombo : MonoBehaviour,IService
    {
        public Sprite[] comboSpites;

        private ParticleSystem _particleSystem;

        private int curentIndex;
        private StatisticManager _statManager;

        public void Init(StatisticManager statisticManager)
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _statManager = statisticManager;
            _statManager.OnCalculateAccuracy += SpawnCombo;
        }

        private void OnDisable()
        {
            _statManager.OnCalculateAccuracy -= SpawnCombo;
        }
        private void OnDestroy()
        {
            _statManager.OnCalculateAccuracy -= SpawnCombo;
        }

        private void SpawnCombo(float accuracy)
        {
            if (curentIndex != _statManager.GetRatingByAccuracyInt(accuracy))
            {
                curentIndex = _statManager.GetRatingByAccuracyInt(accuracy);
                if (curentIndex != -1)
                {
                    _particleSystem.textureSheetAnimation.SetSprite(0, comboSpites[curentIndex]);
                }
            }
            if (curentIndex != -1) _particleSystem.Emit(1);

        }
    }
}