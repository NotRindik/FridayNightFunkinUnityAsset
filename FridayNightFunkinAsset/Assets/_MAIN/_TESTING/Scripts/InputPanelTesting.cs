using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;

public class InputPanelTesting : MonoBehaviour
{
    public InputPanel inputPanel;

    private void Start()
    {
        StartCoroutine(Running());
    }

    IEnumerator Running()
    {
        Character Raelin = CharacterManager.instance.CreateCharacter("Raelin", revealAfterCreation: true);

        yield return Raelin.Say("Hi, What is your name?");

        inputPanel.Show("What Is Your Name?");

        while (inputPanel.isWaitingOnUserInput)
            yield return null;

        string characterName = inputPanel.lastInput;

        yield return Raelin.Say($"It's very nice to meet you,{characterName}!");
    }
}
