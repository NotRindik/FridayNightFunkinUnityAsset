using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class TestConversationQueue : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Running());  
    }

    IEnumerator Running()
    {
        List<string> lines = new List<string>()
        {
            "This is Line 1",
            "This is Line 2",
            "This is Line 3"
        };

        yield return DialogueSystem.instance.Say(lines);

        DialogueSystem.instance.Hide();
    }

    private void Update()
    {
        List<string> lines = new List<string>();
        Conversation conversation = null;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            lines = new List<string>()
            {
                "This is the start of enw..",
                "We cant keep it going!"
            };
            conversation = new Conversation(lines);
            DialogueSystem.instance.conversationManager.Enqueue(conversation);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            lines = new List<string>()
            {
                "This is an important conversation",
                "August 26,2023 is international dog day!"
            };
            conversation = new Conversation(lines);
            DialogueSystem.instance.conversationManager.EnqueuePriority(conversation);
        }
    }
}
