using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace FridayNightFunkin.UI
{
    public class PersonInCredit : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textBox;
        [SerializeField] private string descriptionText;

        private Button button;

        private void Start()
        {
            button = GetComponent<Button>();
        }

        private void Update()
        {
            if (button.gameObject == EventSystem.current.currentSelectedGameObject)
            {
                SetDescription();
            }
        }

        private void SetDescription()
        {
            textBox.text = descriptionText;
        }
    }
}