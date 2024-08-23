using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class DIALOGUE_LINE
    {
        public string rawData { get; private set; } = string.Empty;

        public DL_SPEAKER_DATA speakerData;
        public DL_DIALOGUE_DATA dialogueData;
        public DL_COMMAND_DATA commandsData;

        public bool hasSpeaker => speakerData != null;//speaker != string.Empty;
        public bool hasDialogue => dialogueData != null;
        public bool hasCommands => commandsData != null;

        public DIALOGUE_LINE(string rawLine,string speaker, string dialogue,string commands)
        {
            rawData = rawLine;
            speakerData = (string.IsNullOrWhiteSpace(speaker) ? null : new DL_SPEAKER_DATA(speaker));
            dialogueData = (string.IsNullOrWhiteSpace(dialogue) ? null : new DL_DIALOGUE_DATA(dialogue));
            commandsData = (string.IsNullOrWhiteSpace(commands) ? null : new DL_COMMAND_DATA(commands));
        } 
    }
}