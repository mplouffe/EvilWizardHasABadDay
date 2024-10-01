using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace lvl_0
{
    public class DialogueController : SingletonBase<DialogueController>
    {
        [SerializeField]
        private DialogueWindow m_dialogueWindow;

        private Queue<DialogueLine> m_currentDialogueQueue;
        private Duration m_dialogueDuration;
        private DialogueControllerState m_currentState = DialogueControllerState.Deactivated;
        private bool m_skipRequested;

        protected override void Awake()
        {
            base.Awake();
            m_dialogueDuration = new Duration(2);
            m_currentDialogueQueue = new Queue<DialogueLine>();
        }

        public void StartDialogue(List<DialogueLine> dialogue)
        {
            m_currentState = DialogueControllerState.Active;
            m_currentDialogueQueue.Clear();
            foreach (DialogueLine line in dialogue)
            {
                m_currentDialogueQueue.Enqueue(line);
            }
            DisplayDialogue();
        }

        private void Update()
        {
            if (m_currentState == DialogueControllerState.Active)
            {
                m_dialogueDuration.Update(Time.deltaTime);
                var canSkip = m_skipRequested && m_dialogueDuration.Delta() > 0.1f;

                if (m_dialogueDuration.Elapsed() || canSkip)
                {
                    if (m_currentDialogueQueue.Count == 0)
                    {
                        CloseDialogWindow();
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
                m_dialogueWindow.Saying(nextLine.Speaker, nextLine.Text);
            }
            else
            {
                m_dialogueWindow.Thinking(nextLine.Speaker, nextLine.Text);
            }
            m_dialogueDuration.Reset(nextLine.Duration);
        }

        private void CloseDialogWindow()
        {
            m_currentState = DialogueControllerState.Deactivated;
            m_dialogueWindow.Hide();
            EventBus<DialogueCompleteEvent>.Raise(new DialogueCompleteEvent());
        }

        private enum DialogueControllerState
        {
            Active,
            Deactivated
        }
    }

    public struct DialogueCompleteEvent : IEvent { }

    public struct DialogueAdvanceEvent : IEvent { }

    [Serializable]
    public struct DialogueLine
    {
        public Speaker Speaker;
        public string Text;
        public float Duration;
        public bool IsSpoken;
    }
}