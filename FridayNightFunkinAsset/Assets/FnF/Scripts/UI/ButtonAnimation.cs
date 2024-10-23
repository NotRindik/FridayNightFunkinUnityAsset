using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FridayNightFunkin.UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonAnimation : MonoBehaviour
    {
        private Animator animator;
        private Button button;
        [SerializeField] private string Idle;
        [SerializeField] private string Select;
        [SerializeField] private string Pressed;
        [SerializeField] private string Disabled;

        private float clickAnimTime;
        private bool onButtonClicked;

        public void Start()
        {
            animator = GetComponent<Animator>();
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        public void Update()
        {

            if (onButtonClicked)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName(Pressed))
                {
                    onButtonClicked = false;
                    clickAnimTime = animator.GetCurrentAnimatorStateInfo(0).length;
                }
            }

            if (clickAnimTime <= 0 && !onButtonClicked)
            {
                if (EventSystem.current.currentSelectedGameObject == gameObject)
                {
                    animator.Play(Select);
                }
                else if (!button.interactable)
                {
                    animator.Play(Disabled);
                }
                else
                {
                    animator.Play(Idle);
                }
            }
            clickAnimTime -= Time.deltaTime;
        }

        public void OnClick()
        {
            onButtonClicked = true;
            animator.Play(Pressed);
        }
    }
}