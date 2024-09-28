using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lvl_0
{
    public class DialogueSystemTester : MonoBehaviour, IEventReceiver<DialogueCompleteEvent>
    {
        [SerializeField]
        private Sprite m_firstSpeaker;

        [SerializeField]
        private Sprite m_secondSpeaker;

        protected void OnEnable()
        {
            EventBus<DialogueCompleteEvent>.Register(this);
        }

        protected void OnDisable()
        {
            EventBus<DialogueCompleteEvent>.Register(this);
        }

        [ContextMenu("RunDialogue")]
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
                    Text = "At least I hope it is.",
                    Duration = 2.0f,
                    IsSpoken = true
                }
            };
            DialogueController.Instance.StartDialogue(dialogue, m_secondSpeaker, false);
        }
    }
}
