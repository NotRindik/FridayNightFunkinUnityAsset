using CHARACTERS;
using COMMANDS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE.LogicalLines;

namespace DIALOGUE
{
    public class ConversationManager
    {
        private DialogueSystem dialogueSystem => DialogueSystem.instance;

        private Coroutine process = null;
        public bool isRunning => process != null;
        public bool isOnLogicalLine { get; private set; } = false;

        public TextArchitect architect = null;
        private bool userPromt = false;

        private LogicalLineManager logicalLineManager;

        public Conversation conversation => (conversationQueue.IsEmpty() ? null : conversationQueue.top);
        public int conversationProgress => (conversationQueue.IsEmpty() ? -1 : conversationQueue.top.GetProgress());

        private ConversationQueue conversationQueue;

        public bool allowUserPromts = true;

        public ConversationManager(TextArchitect architect)
        {
            this.architect = architect;
            dialogueSystem.onUserPromt_Next += OnUserPromt_Next;
            logicalLineManager = new LogicalLineManager();

            conversationQueue = new ConversationQueue();
        }

        public Conversation[] GetConversationQueue() => conversationQueue.GetReadOnly();

        public void Enqueue(Conversation conversation) => conversationQueue.Enqueue(conversation);
        public void EnqueuePriority(Conversation conversation) => conversationQueue.EnqueuePriotity(conversation);
        

        private void OnUserPromt_Next()
        {
            if(allowUserPromts)
                userPromt = true;
        }

       public Coroutine StartConversation(Conversation conversation)
       {
            StopConversation();
            conversationQueue.Clear();

            Enqueue(conversation);

            process = dialogueSystem.StartCoroutine(RunningConversation());

            return process;
       }

        public void StopConversation()
        {
            if (!isRunning)
            {
                return;
            }

            dialogueSystem.StopCoroutine(process);
            process = null;
        }

        IEnumerator RunningConversation()
        {
            while(!conversationQueue.IsEmpty())
            {
                Conversation currentConversation = conversation;

                if (currentConversation.HasReachedEnd())
                {
                    conversationQueue.Dequeue();
                    continue;
                }

                string rawLin = currentConversation.CurrentLine();

                if (string.IsNullOrWhiteSpace(rawLin))
                {
                    TryAdvanceConversation(currentConversation);
                    continue;
                }

                DIALOGUE_LINE line = DialogueParser.Parse(rawLin);

                if (logicalLineManager.TryGetLogic(line, out Coroutine logic))
                {
                    isOnLogicalLine = true;
                    yield return logic;
                }
                else
                {
                    if (line.hasDialogue)
                        yield return Line_RunDialogue(line);

                    if (line.hasCommands)
                    {
                        yield return Line_RunCommands(line);
                    }

                    if (line.hasDialogue)
                    {
                        yield return WaitForUserInput();
                        CommandManager.instance.StopAllProcesses();

                        dialogueSystem.OnSystemPromt_Clear();
                    }
                }
                TryAdvanceConversation(currentConversation);
                isOnLogicalLine = false;
            }

            process = null;
        }

        private void TryAdvanceConversation(Conversation conversation)
        {
            conversation.IncrementProgress();

            if (conversation != conversationQueue.top)
                return;

            if (conversation.HasReachedEnd())
                conversationQueue.Dequeue();
        }

        IEnumerator Line_RunDialogue(DIALOGUE_LINE line)
        {
            if (line.hasSpeaker)
                HandleSpeakerLogic(line.speakerData);

            if (!dialogueSystem.dialogueContainer.isVisible)
                dialogueSystem.dialogueContainer.Show();

            yield return BuildLineSegments(line.dialogueData);

        }

        private void HandleSpeakerLogic(DL_SPEAKER_DATA speakerData)
        {
            bool characterMustBeCreated = (speakerData.makeCharacterEnter || speakerData.isCastingPosition || speakerData.isCastingExpressions);

            Character character = CharacterManager.instance.GetCharacter(speakerData.name, createIfDoesNotExist: characterMustBeCreated);

            if (speakerData.makeCharacterEnter && (!character.isVisible && !character.isRevealing))
                character.Show();

            dialogueSystem.ShowSpeakerName(TagManager.Inject(speakerData.displayname));

            DialogueSystem.instance.ApplySpeakerDataToDialogueContainer(speakerData.name);

            if (speakerData.isCastingPosition)
                character.MoveToPosition(speakerData.castPosition);

            if (speakerData.isCastingExpressions)
            {
                foreach (var ce in speakerData.CastExpressions)
                    character.OnReceiveCastingExpression(ce.layer, ce.expression);
            }
        }
        IEnumerator Line_RunCommands(DIALOGUE_LINE line)
        {
            List<DL_COMMAND_DATA.Command> commands = line.commandsData.commands;

            foreach (DL_COMMAND_DATA.Command command in commands)
            {
                if (command.waitForCompletion || command.name == "wait")
                {
                    CoroutineWrapper cw = CommandManager.instance.Execute(command.name, command.arguments);
                    while (!cw.isDone)
                    {
                        if (userPromt)
                        {
                            CommandManager.instance.StopCurrentProcess();
                            userPromt = false;
                        }
                        yield return null;
                    }
                }
                else
                    CommandManager.instance.Execute(command.name, command.arguments);//a
            }
            yield return null;
        }

        IEnumerator BuildLineSegments(DL_DIALOGUE_DATA line)
        {
            for (int i = 0; i < line.segments.Count; i++)
            {
                DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment = line.segments[i];

                yield return WaitForDialogueSegmentSignalToBeTriggered(segment);

                yield return BuildDialogue(segment.dialogue, segment.appendText);
            }
        }

        public bool isWaitingOnAutoTimer { get; private set; } = false;

        IEnumerator WaitForDialogueSegmentSignalToBeTriggered(DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment)
        {
            switch (segment.startSignal)
            {
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.C:
                    yield return WaitForUserInput();
                    dialogueSystem.OnSystemPromt_Clear();
                    break;

                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.A:
                    yield return WaitForUserInput();
                    break;
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WC:
                    isWaitingOnAutoTimer = true;
                    yield return new WaitForSeconds(segment.signalDelay);
                    isWaitingOnAutoTimer = false;
                    dialogueSystem.OnSystemPromt_Clear();
                    break;
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WA:
                    isWaitingOnAutoTimer = true;
                    yield return new WaitForSeconds(segment.signalDelay);
                    isWaitingOnAutoTimer = false;
                    break;
                default:
                    break;
            }
        }

        IEnumerator BuildDialogue(string dialogue,bool append = false)
        {
            dialogue = TagManager.Inject(dialogue);

            if (!append)
                architect.Build(dialogue);
            else
                architect.Append(dialogue);

            while (architect.isBuilding)
            {
                if (userPromt)
                {
                    if (!architect.hurryUp)
                    {
                        architect.hurryUp = true;
                    }
                    else
                    {
                        architect.ForceComplete();
                    }

                    userPromt = false;
                }
                yield return null;
            }
        }

        IEnumerator WaitForUserInput()
        {
            dialogueSystem.prompt.Show();

            while (!userPromt)        
                yield return null;

            dialogueSystem.prompt.Hide();

            userPromt = false;
        }
    }
}