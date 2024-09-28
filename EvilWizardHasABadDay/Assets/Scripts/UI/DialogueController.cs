using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace lvl_0
{
    public class DialogueController : SingletonBase<DialogueController>
    {
        private Duration m_dialogueDuration;

        private DialogueControllerState m_currentState;

        private Queue<DialogueLine> m_currentDialogueQueue;

        [SerializeField]
        private DialogueWindow m_dialogueWindow;

        private bool m_skipRequested;
        private Sprite m_currentSpeaker;
        private bool m_keepWindowOpenAfterDialogue;

        protected override void Awake()
        {
            base.Awake();
            m_dialogueDuration = new Duration(2);
            m_currentDialogueQueue = new Queue<DialogueLine>();
        }

        public void StartDialogue(List<DialogueLine> dialogue, Sprite speaker, bool expectsResponse)
        {
            m_currentState = DialogueControllerState.DisplayingDialogue;
            m_currentSpeaker = speaker;
            m_currentDialogueQueue.Clear();
            foreach (DialogueLine line in dialogue)
            {
                m_currentDialogueQueue.Enqueue(line);
            }
            m_keepWindowOpenAfterDialogue = expectsResponse;
            DisplayDialogue();
        }

        private void Update()
        {
            if (m_currentState == DialogueControllerState.DisplayingDialogue)
            {
                m_dialogueDuration.Update(Time.deltaTime);
                var canSkip = m_skipRequested && m_dialogueDuration.Delta() > 0.1f;

                if (m_dialogueDuration.Elapsed() || canSkip)
                {
                    if (m_currentDialogueQueue.Count == 0)
                    {
                        if (m_keepWindowOpenAfterDialogue)
                        {
                            m_currentState = DialogueControllerState.AwaitingResponse;
                            EventBus<DialogueCompleteEvent>.Raise(new DialogueCompleteEvent());
                        }
                        else
                        {
                            CloseDialogWindow();
                        }
                    }
                    else
                    {

                        DisplayDialogue();
                    }
                    m_skipRequested = false;
                }
            }
        }

        private void DisplayDialogue()
        {
            var nextLine = m_currentDialogueQueue.Dequeue();

            if (nextLine.IsSpoken)
            {
                m_dialogueWindow.Saying(nextLine.Speaker, nextLine.Text, m_currentSpeaker);
            }
            else
            {
                m_dialogueWindow.Thinking(nextLine.Speaker, nextLine.Text, m_currentSpeaker);
            }
            m_dialogueDuration.Reset(nextLine.Duration);
        }

        private void CloseDialogWindow()
        {
            m_currentState = DialogueControllerState.Deactivated;
            m_dialogueWindow.Hide();
        }

        private enum DialogueControllerState
        {
            DisplayingDialogue,
            Deactivated,
            AwaitingResponse
        }
    }

    public struct DialogueCompleteEvent : IEvent { }

    public struct DialogueLine
    {
        public Speaker Speaker;
        public string Text;
        public float Duration;
        public bool IsSpoken;
    }
}