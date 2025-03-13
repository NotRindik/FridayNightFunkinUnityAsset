using FridayNightFunkin.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using FnF.Scripts.Extensions;
using UnityEngine;

namespace FridayNightFunkin.Calculations
{
    public class StatisticManager : MonoBehaviour,IService
    {
        public int score { get; private set; }

        public int combo { get; private set; }

        public int misses { get; private set; }

        public float totalAccuracy { get; private set; }

        public List<int> accuracyList = new List<int>();

        public Action OnValueChanged;
        public Action<float> OnCalculateAccuracy;
        
        public int CalculateAccuracy(float distance)
        {
            if (distance < 20) distance = 0;

            int accuracy = Mathf.RoundToInt(100 * (1 - (distance/500)));
            accuracyList.Add(accuracy);
            OnValueChanged?.Invoke();
            OnCalculateAccuracy?.Invoke(accuracy);
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
            OnValueChanged?.Invoke();
        }
        public float CalculateTotalAccuracy(List<int> Accuracy)
        {
            int multiplyOfList = 0;
            foreach (var item in Accuracy)
            {
                multiplyOfList += item;
            }

            totalAccuracy = Mathf.RoundToInt(multiplyOfList / Accuracy.Count);
            OnValueChanged?.Invoke();
            return totalAccuracy;
        }

        public void AddMiss()
        {
            misses++;
            OnValueChanged?.Invoke();
        }

        public void AddCombo()
        {
            combo++;
            OnValueChanged?.Invoke();
        }

        public void ResetCombo()
        {
            combo = 0;
            OnValueChanged?.Invoke();
        }
    }
}