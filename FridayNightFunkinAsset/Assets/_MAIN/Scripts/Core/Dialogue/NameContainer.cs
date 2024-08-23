using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace DIALOGUE
{
    [System.Serializable]
    public class NameContainer
    {
        [SerializeField] private GameObject root;
        [field:SerializeField] public TextMeshProUGUI nameText { get; private set; }
        public void Show(string nameToShow = "")
        {
            root.SetActive(true);

            if (nameToShow != string.Empty)
                 nameText.text = nameToShow;
        }

        public void Hide()
        {
            root.SetActive(false);
        }

        public void SetNameColor(Color color) => nameText.color = color;
        public void SetNameFont(TMP_FontAsset font) => nameText.font = font;
         
        public void SetNameFontSize(float size) => nameText.fontSize = size;
    }
}