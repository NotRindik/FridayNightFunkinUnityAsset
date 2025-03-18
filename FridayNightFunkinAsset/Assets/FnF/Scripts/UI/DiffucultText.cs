using System;
using FridayNightFunkin.Editor.TimeLineEditor;
using TMPro;
using UnityEngine;
namespace FnF.Scripts.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class DiffucultText : MonoBehaviour
    {
        public TextMeshProUGUI difficultText;
        private DifficultLevel _difficultLevel;

        private void OnValidate()
        {
            if (!difficultText)
            {
                difficultText = GetComponent<TextMeshProUGUI>();
            }
        }

        private void Start()
        {
            _difficultLevel = (DifficultLevel)ChartPlayBack.CurrentDifficult;
            difficultText.text = _difficultLevel.ToString();
        }
    }
}
