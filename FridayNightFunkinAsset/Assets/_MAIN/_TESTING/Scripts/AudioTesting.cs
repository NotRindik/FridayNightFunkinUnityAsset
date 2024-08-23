using CHARACTERS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING
{
    public class AudioTesting : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Running2());
        }

        Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);
        IEnumerator Running()
        {

                Character_Sprite Raelin = CreateCharacter("Raelin") as Character_Sprite;
                Raelin.Show();

                AudioManager.instance.PlaySoundEffect("Audio/SFX/RadioStatic", loop: true);

                yield return Raelin.Say("I'm going to turn off the radio");

                AudioManager.instance.StopSoundEffect("RadioStatic");

                Raelin.Say("It's off now");
            
        }

        IEnumerator Running2()
        {
            AudioManager.instance.PlayTrack("Audio/Music/Calm",0);

            yield return new WaitForSeconds(2f);

            AudioManager.instance.PlayTrack("Audio/Music/Comedy",1, pitch: 0.7f);

            yield return new WaitForSeconds(5f);

            AudioManager.instance.StopTrack(0);

            yield return null;
        }
    }
}