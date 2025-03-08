using FridayNightFunkin.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FridayNightFunkin.Calculations
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager instance { get; private set; }

        public int score { get; private set; }

        internal int combo { get; private set; }

        internal int misses { get; private set; }

        internal float totalAccuracy { get; private set; }

        public List<int> accuracyList = new List<int>();

        internal bool isDead;

        private Coroutine sliderMoveProcess;


        public event Action<int> OnCalculateAccuracy;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.LogWarning($"Dont create 2 {this} pls!!!!");
                DestroyImmediate(gameObject);
            }
        }
        public void AddValueToSlider(float value)
        {
            if (value < 0)
            {
                Debug.LogWarning("Bro, this shit is ADD VALUE, NOT REDUSE U FUCKING SHIT!!, DONT PUT NEGATIVE NUMBERS");
                return;
            }

            StartMoveSliderSmoothly(FNFUIElement.instance.versusSlider.value + value, 1f);
        }

        public void StartMoveSliderSmoothly(float targetValue, float initialAdder)
        {
            if (sliderMoveProcess != null)
            {
                StopCoroutine(sliderMoveProcess);
            }

            sliderMoveProcess = StartCoroutine(MoveSliderSmoothlyCoroutine(targetValue, initialAdder));
        }

        public IEnumerator MoveSliderSmoothlyCoroutine(float targetValue, float initialAdder)
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();

                FNFUIElement.instance.versusSlider.value = Mathf.MoveTowards(FNFUIElement.instance.versusSlider.value, targetValue, initialAdder);

                if (FNFUIElement.instance.versusSlider.value == targetValue)
                {
                    break;
                }
            }
        }

        public void ReduceValueToSlider(float value)
        {
            if (value < 0)
            {
                Debug.LogWarning("AAAAAAAAAAAAAAAAA!!!!! FUCKIN LITLE SHIT, THIS METHOD IS REDUCE VALUE, REDUCE!!! U KNOW WHEN - to - equals +!! - to - is +, SO WHY U PUT NEGATIVE MUMBER");
                return;
            }
            StartMoveSliderSmoothly(FNFUIElement.instance.versusSlider.value - value, 1f);
        }
        public void ReduceValueToSliderEnemy(float value)
        {
            if (value < 0)
            {
                Debug.LogWarning("AAAAAAAAAAAAAAAAA!!!!! FUCKIN LITLE SHIT, THIS METHOD IS REDUCE VALUE, REDUCE!!! U KNOW WHEN - to - equals +!! - to - is +, SO WHY U PUT NEGATIVE MUMBER");
                return;
            }
            if(FNFUIElement.instance.versusSlider.value - value <= FNFUIElement.instance.versusSlider.minValue + 1) 
            {
                StartMoveSliderSmoothly(FNFUIElement.instance.versusSlider.minValue + 1,1);
            }
            else if (FNFUIElement.instance.versusSlider.value > FNFUIElement.instance.versusSlider.minValue)
            {
                StartMoveSliderSmoothly(FNFUIElement.instance.versusSlider.value - value, 1);
            }
        }

        public int CalculateAccuracy(float distance)
        {
            if (distance < 20) distance = 0;

            int accuracy = Mathf.RoundToInt(100 * (1 - (distance/500)));
            OnCalculateAccuracy?.Invoke(accuracy);
            accuracyList.Add(accuracy);
            return accuracy;
        }

        public string GetRatingByAccuracy(float accuracy)
        {
            if (accuracy == 100)
                return "Perfect!!";
            else if (accuracy > 90)
                return "Sick!";
            else if (accuracy > 80)
                return "Great";
            else if (accuracy > 70)
                return "Good";
            else if (accuracy > 68)
                return "Nice";
            else if (accuracy > 60)
                return "Meh";
            else if (accuracy > 50)
                return "Bruh";
            else if (accuracy > 40)
                return "Bad";
            else if (accuracy > 20)
                return "Shit";
            else
                return "You Suck!";
        }

        public int GetRatingByAccuracyInt(float accuracy)
        {
            if (accuracy == 100)
                return 0;
            else if (accuracy > 90)
                return 1;
            else if (accuracy > 80)
                return 1;
            else if (accuracy > 70)
                return 2;
            else if (accuracy > 68)
                return 2;
            else if (accuracy > 60)
                return 2;
            else if (accuracy > 50)
                return 2;
            else if (accuracy > 40)
                return 3;
            else if (accuracy > 20)
                return 3;
            else
                return -1;
        }

        public void AddScore(uint addingScore)
        {
            score += (int)addingScore;
        }
        public float CalculateTotalAccuracy(List<int> Accuracy)
        {
            int multiplyOfList = 0;
            foreach (var item in Accuracy)
            {
                multiplyOfList += item;
            }

            totalAccuracy = Mathf.RoundToInt(multiplyOfList / Accuracy.Count);

            return totalAccuracy;
        }

        public void AddMiss()
        {
            misses++;
            FNFUIElement.instance.UpdateUI();
        }

        public void AddCombo()
        {
            combo++;
            FNFUIElement.instance.UpdateUI();
        }

        public void ResetCombo()
        {
            combo = 0;
            FNFUIElement.instance.UpdateUI();
        }
    }
}