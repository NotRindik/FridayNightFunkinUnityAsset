using UnityEngine;
using UnityEngine.UI;

namespace FridayNightFunkin.UI
{
    public class CheckBoxAnimation : MonoBehaviour
    {
        private Animator animator;
        private Toggle toggle;
        [SerializeField] private string IdleUnCheck;
        [SerializeField] private string IdleCheck;
        [SerializeField] private string Pressed;
        [SerializeField] private string UnPressed;
        [SerializeField] private string Disabled;

        private float pressingAnimTime;
        private bool isTimerStart;
        private bool isTick;

        public void Start()
        {
            animator = GetComponent<Animator>();
            toggle = GetComponent<Toggle>();

            isTick = toggle.isOn;
            toggle.onValueChanged.AddListener(OnClick);
        }

        public void Update()
        {
            if ((animator.GetCurrentAnimatorStateInfo(0).IsName(Pressed) || animator.GetCurrentAnimatorStateInfo(0).IsName(UnPressed)) && isTimerStart)
            {
                pressingAnimTime = animator.GetCurrentAnimatorStateInfo(0).length;
                isTimerStart = false;
            }
            if (pressingAnimTime <= 0 && !isTimerStart)
            {
                if (isTick)
                {
                    animator.Play(IdleCheck);
                }
                else
                {
                    animator.Play(IdleUnCheck);
                }
            }
            pressingAnimTime -= Time.deltaTime;
        }

        public void OnClick(bool value)
        {
            isTick = value;
            isTimerStart = true;
            if (isTick)
            {
                animator.Play(Pressed);
            }
            else
            {
                animator.Play(UnPressed);
            }
        }
    }
}