using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FridayNightFunkin.CHARACTERS
{
    public class Character_Fnf_PlayAble : Ñharacter_FNF
    {
        private string[] MiSS_ANIMATION = { "LeftFail", "DownFail", "UpFail", "DownFail" };

        private void Start()
        {
            LevelSettings.instance.player = this;
        }

        public void PlayMissAnimation(Arrow arrow)
        {
            animator.CrossFade(MiSS_ANIMATION[(int)arrow.arrowSide], 0);

            LevelSettings.instance.playerInput.SetIdleAnim(0.4f);
        }
    }
}