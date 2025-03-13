using Cinemachine;
using UnityEngine;

public class CharacterCamera : MonoBehaviour
{
    public CharacterSide characterSide = CharacterSide.Player;

    public CinemachineVirtualCamera cinemachineVirtualCamera;

    public void Init(MapSpawner mapSpawner)
    {
        cinemachineVirtualCamera.Follow = mapSpawner.GetCharacterPos(characterSide, 0).transform ;
    }

    private void OnValidate()
    {
        if (!cinemachineVirtualCamera)
        {
            TryGetComponent(out cinemachineVirtualCamera);
        }
    }
}
