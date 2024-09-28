using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lvl_0
{
    public class GameSystemsTester : MonoBehaviour, IEventReceiver<DialogueCompleteEvent>, IEventReceiver<QTEEvent>
    {
        [SerializeField]
        private Sprite m_firstSpeaker;

        [SerializeField]
        private Sprite m_secondSpeaker;

        private int test = 0;

        protected void OnEnable()
        {
            EventBus<DialogueCompleteEvent>.Register(this);
            EventBus<QTEEvent>.Register(this);
        }

        protected void OnDisable()
        {
            EventBus<DialogueCompleteEvent>.Unregister(this);
            EventBus<QTEEvent>.Unregister(this);
        }

        [ContextMenu("Start Test")]
        public void RunDialogue()
        {
            var dialogue = new List<DialogueLine>()
            {
                new DialogueLine()
                {
                    Speaker = Speaker.Wizard,
                    Text = "Hello World!",
                    Duration = 2.0f,
                    IsSpoken = true
                },
                new DialogueLine()
                {
                    Speaker = Speaker.Wizard,
                    Text = "This is a test of my dialogue system...",
                    Duration = 2.0f,
                    IsSpoken = true
                },
                new DialogueLine()
                {
                    Speaker = Speaker.Wizard,
                    Text = "How you like me now???",
                    Duration = 2.0f,
                    IsSpoken = true
                }
            };
            DialogueController.Instance.StartDialogue(dialogue, m_firstSpeaker, true);
        }

        public void OnEvent(DialogueCompleteEvent e)
        {
            Debug.Log("DialogueCompleteEvent");
            if (test == 0)
            {
                var dialogue = new List<DialogueLine>()
                {
                    new DialogueLine()
                    {
                        Speaker = Speaker.Other,
                        Text = "Yes yes.",
                        Duration = 2.0f,
                        IsSpoken = true
                    },
                    new DialogueLine()
                    {
                        Speaker = Speaker.Other,
                        Text = "This seems to be working...",
                        Duration = 2.0f,
                        IsSpoken = true
                    },
                    new DialogueLine()
                    {
                        Speaker = Speaker.Other,
                        Text = "Care for a quicktime event?",
                        Duration = 2.0f,
                        IsSpoken = true
                    }
                };

                test = 1;
                DialogueController.Instance.StartDialogue(dialogue, m_secondSpeaker, false);
            }
            else if (test == 1)
            {
                Debug.Log("Triggering QTE..");
                QuicktimeEventManager.Instance.StartQTE(5.0f, new List<KeyCode>()
                {
                    KeyCode.UpArrow,
                    KeyCode.DownArrow,
                    KeyCode.UpArrow,
                    KeyCode.DownArrow,
                    KeyCode.UpArrow,
                    KeyCode.DownArrow,
                    KeyCode.UpArrow,
                    KeyCode.DownArrow,

                });
            }
        }

        public void OnEvent(QTEEvent e)
        {
            if (e.Failed)
            {
                var dialogue = new List<DialogueLine>()
                {
                    new DialogueLine()
                    {
                        Speaker = Speaker.Other,
                        Text = "Rough show old chap.",
                        Duration = 2.0f,
                        IsSpoken = true
                    },
                    new DialogueLine()
                    {
                        Speaker = Speaker.Other,
                        Text = "Care to try that again?",
                        Duration = 2.0f,
                        IsSpoken = true
                    },
                };
                DialogueController.Instance.StartDialogue(dialogue, m_secondSpeaker, false);
            }
            else
            {
                var dialogue = new List<DialogueLine>()
                {
                    new DialogueLine()
                    {
                        Speaker = Speaker.Other,
                        Text = "Well done!",
                        Duration = 2.0f,
                        IsSpoken = true
                    },
                    new DialogueLine()
                    {
                        Speaker = Speaker.Other,
                        Text = "Jolly good show...",
                        Duration = 2.0f,
                        IsSpoken = true
                    },
                };
                test = 2;
                DialogueController.Instance.StartDialogue(dialogue, m_secondSpeaker, false);
            }
        }
    }
}
