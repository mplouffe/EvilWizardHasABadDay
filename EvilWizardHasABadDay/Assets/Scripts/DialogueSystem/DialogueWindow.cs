using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

namespace lvl_0
{
    public enum Speaker
    {
        None,
        Wizard,
        Other
    }

    public class DialogueWindow : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup m_canvasGroup;

        [SerializeField]
        private Image m_imageIcon;

        [SerializeField]
        private RectTransform m_imageIconRectTransform;

        [SerializeField]
        private TextMeshProUGUI m_dialogueTextField;

        [SerializeField]
        private HeadshotCatalogue m_dialogueIcons;

        private Speaker m_currentSpeaker;
        private bool m_iconIsHidden;

        private InputActions m_inputActions;

        protected void OnEnable()
        {
            m_inputActions = new InputActions();
            Hide();
        }

        protected void OnDisable()
        {
            m_inputActions.Dialogue.Confirm.performed += OnConfirmPressed;
        }

        public void DisplayDialogue(DialogueLine line)
        {
            UpdateSpeaker(line.Speaker);
            m_dialogueTextField.fontStyle = line.IsSpoken ? FontStyles.Normal : FontStyles.Italic;
            m_dialogueTextField.text = line.Text;
        }

        public void Hide()
        {
            m_dialogueTextField.text = string.Empty;
            m_canvasGroup.alpha = 0;
            m_iconIsHidden = true;
            m_inputActions.Dialogue.Disable();
        }

        public void Show()
        {
            m_canvasGroup.alpha = 1;
            m_iconIsHidden = false;
            m_inputActions.Dialogue.Enable();
        }

        private void OnConfirmPressed(CallbackContext context)
        {
            AdvanceDialogue();
        }

        private void AdvanceDialogue()
        {
            EventBus<DialogueAdvanceEvent>.Raise(new DialogueAdvanceEvent());
        }

        private void PositionIcon(bool onLeft)
        {
            if (onLeft)
            {
                m_imageIconRectTransform.anchoredPosition = new Vector2(-793, m_imageIconRectTransform.anchoredPosition.y);
                m_imageIconRectTransform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                m_imageIconRectTransform.anchoredPosition = new Vector2(793, m_imageIconRectTransform.anchoredPosition.y);
                m_imageIconRectTransform.localScale = new Vector3(-1, 1, 1);
            }
        }

        private void UpdateSpeaker(Speaker speaker)
        {
            if (m_currentSpeaker != speaker)
            {
                m_imageIcon.sprite = m_dialogueIcons.GetActorIcon(speaker);
                PositionIcon(speaker == Speaker.Wizard);
                m_currentSpeaker = speaker;
            }

            if (m_iconIsHidden)
            {
                Show();
            }
        }
    }




}
