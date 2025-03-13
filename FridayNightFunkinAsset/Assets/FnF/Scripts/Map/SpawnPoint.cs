using UnityEngine;
namespace FnF.Scripts.Map
{
    [DisallowMultipleComponent]
    public class SpawnPoint : MonoBehaviour
    {
        [Tooltip("You can use it to mark the side of character")]
        public CharacterSide characterSide = CharacterSide.Player;
    }
}
