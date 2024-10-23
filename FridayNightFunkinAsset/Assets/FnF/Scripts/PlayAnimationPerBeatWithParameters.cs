using UnityEngine;

namespace FridayNightFunkin
{
    public class PlayAnimationPerBeatWithParameters : PlayAnimPerBeat
    {
        [SerializeField] private string triggerName = "Triggered";
        protected override void PlayAnimation()
        {
            if (time >= 1 / BPS)
            {
                time = 0;
                animator.SetTrigger(triggerName);
            }
        }
    }
}