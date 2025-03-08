using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FridayNightFunkin.CHARACTERS
{
    public class Character_Fnf_Girlfriend: Сharacter_FNF
    {
        internal CharacterSide characterSide = CharacterSide.Gf;
        public override RoadSide roadSide => RoadSide.Player;
    }
}