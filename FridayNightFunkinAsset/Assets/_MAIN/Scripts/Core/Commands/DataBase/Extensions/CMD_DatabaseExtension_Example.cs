using COMMANDS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING
{
    public class CMD_DatabaseExtension_Example : CMD_DatabaseExtension
    {
        new public static void Extend(CommandDatabase dataBase)
        {
            dataBase.AddCommand("print", new Action(PrintDefaultMessage));
            dataBase.AddCommand("print_1p", new Action<string>(PrintUsermessage));
            dataBase.AddCommand("print_mp", new Action<string[]>(PrintLines));

            dataBase.AddCommand("lambda", new Action(() => { Debug.Log("Printing a default message to console from lambda command."); }));
            dataBase.AddCommand("lambda_1p", new Action<string>((arg) => { Debug.Log($"Log User Lambda Message: '{arg}'"); }));
            dataBase.AddCommand("lambda_mp", new Action<string[]>((args) => { Debug.Log(string.Join(", ", args)); }));

            dataBase.AddCommand("process", new Func<IEnumerator>(SimpleProcess));
            dataBase.AddCommand("process_1p", new Func<string, IEnumerator>(LineProcess));
            dataBase.AddCommand("process_mp", new Func<string[], IEnumerator>(MultiLineProcces));

            dataBase.AddCommand("moveCharDemo", new Func<string, IEnumerator>(MoveCharacter));
        }

        private static void PrintDefaultMessage()
        {
            Debug.Log("Printing a default message to console.");
        }

        private static void PrintUsermessage(string message)
        {
            Debug.Log($"User Message: '{message}'");
        }


        private static void PrintLines(string[] lines)
        {
            int i = 1;
            foreach (string line in lines)
            {
                Debug.Log($"{i++}. '{line}'");
            }
        }

        private static IEnumerator SimpleProcess()
        {
            for (int i = 0; i < 5; i++)
            {
                Debug.Log($"Process Running... [{i}]");
                yield return new WaitForSeconds(1);
            }
        }
        private static IEnumerator LineProcess(string data)
        {
            if (int.TryParse(data, out int num))
            {
                for (int i = 0; i < num; i++)
                {
                    Debug.Log($"Process Running... [{i}]");
                    yield return new WaitForSeconds(1);
                }
            }
        }
        private static IEnumerator MultiLineProcces(string[] data)
        {
            foreach (string line in data)
            {
                Debug.Log($"Process Message: '{line}'");
                yield return new WaitForSeconds(0.5f);
            }
        }

        private static IEnumerator MoveCharacter(string direction)
        {
            bool left = direction.ToLower() == "left";

            Transform character = GameObject.Find("Image").transform;
            float moveSpeed = 15;

            float targetX = left ? -8 : 8;

            float currentX = character.position.x;

            while (Mathf.Abs(targetX - currentX) > 0.1f)
            {
                currentX = Mathf.MoveTowards(currentX, targetX, moveSpeed * Time.deltaTime);
                character.position = new Vector3(currentX, character.position.y, character.position.z);
                yield return null;
            }
        }
    }
}