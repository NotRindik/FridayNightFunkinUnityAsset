using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using DIALOGUE;
using TMPro;

namespace TESTING
{
    public class TestCharacters : MonoBehaviour
    {
        public TMP_FontAsset tempFont;
        private Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name); 
        void Start()
        {
            //Character Raelin = CharacterManager.instance.CreateCharacter("Raelin");
            //Character Maxim = CharacterManager.instance.CreateCharacter("Maxim");
            //Character Adam = CharacterManager.instance.CreateCharacter("Adam");
            //StartCoroutine(Test());
            StartCoroutine(Test4());
        }

        IEnumerator Test()
        {
            //Character_Sprite guard1 = CreateCharacter("Guard1 as Generic") as Character_Sprite;
            Character_Sprite Raelin = CreateCharacter("Raelin") as Character_Sprite;
            //Character fs = CreateCharacter("Female Student 2");
            Character_Live2D Mao = CreateCharacter("Mao") as Character_Live2D;;

            Raelin.SetPosition(new Vector2(0, 0));
            Mao.SetPosition(new Vector2(0.5f,0));

            yield return new WaitForSeconds(0.5f);
            Mao.TransitionColor(Color.red);
            yield return new WaitForSeconds(0.5f);
            Mao.TransitionColor(Color.white);

            yield return null;
        }

        IEnumerator Test2()
        {
            Character_Model3D Kiki = CreateCharacter("Kiki") as Character_Model3D;

            yield return new WaitForSeconds(1f);

            yield return Kiki.MoveToPosition(new Vector2(1, 0));

            Kiki.SetExpression("Sad", 100);
        }
        IEnumerator Test3()
        {
            Character_Model3D Kiki = CreateCharacter("Kiki") as Character_Model3D;

            yield return new WaitForSeconds(1f);

            Kiki.SetColor(Color.red);

            yield return new WaitForSeconds(1f);

            yield return Kiki.Hide();
        }
        IEnumerator Test4()
        {
            Character_Sprite Raelin = CreateCharacter("Raelin") as Character_Sprite;
            Character_Sprite Guard1 = CreateCharacter("Guard as Generic") as Character_Sprite;
            Raelin.Show();
            Guard1.Show();
            Raelin.SetPosition(new Vector2(0, 0));

            yield return new WaitForSeconds(1f);

            Raelin.UnHighlight();

            yield return new WaitForSeconds(1f);

            Raelin.Highlight();
        }
    }
}