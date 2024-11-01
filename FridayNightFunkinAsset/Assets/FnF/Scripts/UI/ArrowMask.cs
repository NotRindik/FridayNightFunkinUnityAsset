using FridayNightFunkin.Settings;
using UnityEngine;

namespace FridayNightFunkin.UI
{
    public class ArrowMask : MonoBehaviour
    {
        public SpriteMask[] playerMask;
        public SpriteMask[] EnemyMask;
        public static ArrowMask instance { get; private set; }
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

        private void Start()
        {
            for (int i = 0; i < playerMask.Length; i++)
            {
                playerMask[i].transform.localPosition = new Vector2(playerMask[i].transform.localPosition.x, playerMask[i].transform.localPosition.y * (ChangesByGameSettings.instance.downscroll == 0 ? 1 : -1));
                EnemyMask[i].transform.localPosition = new Vector2(EnemyMask[i].transform.localPosition.x, EnemyMask[i].transform.localPosition.y * (ChangesByGameSettings.instance.downscroll == 0 ? 1 : -1));
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