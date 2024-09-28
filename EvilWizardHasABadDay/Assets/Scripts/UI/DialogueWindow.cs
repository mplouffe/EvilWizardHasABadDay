using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace lvl_0
{
    public class DialogueWindow : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup m_canvasGroup;

        [SerializeField]
        private Image m_imageIcon;

        [SerializeField]
        private RectTransform m_imageIconRectTransform;

        [SerializeField]
        private RectTransform m_textBoxBackgroundRectTransform;

        [SerializeField]
        private TextMeshProUGUI m_dialogueTextField;

        private Speaker m_currentSpeaker;
        private bool m_iconIsHidden;

        protected void OnEnable()
        {
            Hide();
        }

        public void Saying(Speaker speaker, string newText, Sprite icon)
        {
            UpdateSpeaker(speaker, icon);
            m_dialogueTextField.fontStyle = FontStyles.Normal;
            m_dialogueTextField.text = newText;
        }

        public void Thinking(Speaker speaker, string newText, Sprite icon)
        {
            UpdateSpeaker(speaker, icon);
            m_dialogueTextField.fontStyle = FontStyles.Italic;
            m_dialogueTextField.text = newText;
        }

        public void Hide()
        {
            m_dialogueTextField.text = string.Empty;
            m_canvasGroup.alpha = 0;
            m_iconIsHidden = true;
        }

        public void Show()
        {
            m_canvasGroup.alpha = 1;
            m_iconIsHidden = false;
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

        private void UpdateSpeaker(Speaker speaker, Sprite icon)
        {
            if (m_currentSpeaker != speaker)
            {
                m_imageIcon.sprite = icon;
                PositionIcon(speaker == Speaker.Wizard);
                m_currentSpeaker = speaker;
            }

            if (m_iconIsHidden)
            {
                Show();
            }
        }
    }

    public enum Speaker
    {
        None,
        Wizard,
        Other
    }
}
