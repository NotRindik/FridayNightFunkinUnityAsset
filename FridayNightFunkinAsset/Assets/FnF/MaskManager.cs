using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FridayNightFunkin.UI
{
    public class MaskManager : MonoBehaviour
    {
        public SpriteMask[] playerMask;
        public SpriteMask[] EnemyMask;
        public static MaskManager instance { get; private set; }
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        public void ActivateMask(int index, CharacterSide side = CharacterSide.Player)
        {
            if (side == CharacterSide.Player)
                playerMask[index].enabled = true;
            else
                EnemyMask[index].enabled = true;
        }
        public void DisActivateMask(int index, CharacterSide side = CharacterSide.Player)
        {
            if (side == CharacterSide.Player)
                playerMask[index].enabled = false;
            else
                EnemyMask[index].enabled = false;
        }
    }
}
public enum CharacterSide
{
    Player,
    Enemy,
    Gf
}