using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Slider = UnityEngine.UI.Slider;

namespace lvl_0
{
    public class QuicktimeEventManager : SingletonBase<QuicktimeEventManager>
    {
        private InputActions m_inputActions;

        [SerializeField]
        private CanvasGroup m_qteCanvasGroup;

        [SerializeField]
        private Slider m_progressBar;

        [SerializeField]
        private TextMeshProUGUI m_progressText;

        [SerializeField]
        private List<QTEButton> m_qteButtons;

        [SerializeField]
        private RectTransform m_qteButtonArea;

        [SerializeField]
        private List<Image> m_qteButtonImages;

        private Dictionary<KeyCode, Sprite> m_spriteDictionary;

        private Duration m_qteDuration;

        private bool qteEventInProgress;

        private Queue<KeyCode> m_requiredInputs;

        private int m_numberOfInputs;

        // private Queue<KeyCode> m_reversedInputs;

        protected void Start()
        {
            m_inputActions = new InputActions();
            m_qteDuration = new Duration();
            m_spriteDictionary = new Dictionary<KeyCode, Sprite>();
            m_requiredInputs = new Queue<KeyCode>();
            foreach(var qteButton in m_qteButtons)
            {
                m_spriteDictionary.Add(qteButton.KeyCode, qteButton.Image);
            }
            ResetQTE();
            m_qteCanvasGroup.alpha = 0;
        }

        protected void Update()
        {
            if (qteEventInProgress)
            {
                m_qteDuration.Update(Time.deltaTime);
                if (m_qteDuration.Elapsed())
                {
                    EndQTE(true);
                }
                else
                {
                    m_progressText.text = m_qteDuration.Remaining().ToString("0.00");
                    m_progressBar.value = m_qteDuration.Delta();

                    CheckRequiredInput();
                    if (m_requiredInputs.Count == 0)
                    {
                        EndQTE(false);
                    }
                }
            }
        }

        public void StartQTE(float qteDuration, List<KeyCode> inputs)
        {
            m_qteDuration.Reset(qteDuration);
            ResetQTE();
            m_qteButtonArea.sizeDelta = new Vector2(inputs.Count * 120, m_qteButtonArea.rect.height);

            for (var i = 0; i < inputs.Count; i++)
            {
                var buttonImage = m_qteButtonImages[i];
                buttonImage.gameObject.SetActive(true);
                buttonImage.sprite = m_spriteDictionary[inputs[i]];
                buttonImage.color = Color.white;

                m_requiredInputs.Enqueue(inputs[i]);
            }

            m_inputActions.QTE.Enable();
            m_qteCanvasGroup.alpha = 1;
            qteEventInProgress = true;
            m_numberOfInputs = inputs.Count;
        }

        private void CheckRequiredInput()
        {
            var input = m_inputActions.QTE.Move.ReadValue<Vector2>();
            switch (m_requiredInputs.Peek())
            {
                case KeyCode.UpArrow:
                    if (input.y >= 0.9) UpdateInputs();
                    break;
                case KeyCode.DownArrow:
                    if (input.y <= -0.9) UpdateInputs();
                    break;
                case KeyCode.LeftArrow:
                    if (input.x <= -0.9) UpdateInputs();
                    break;
                case KeyCode.RightArrow:
                    if (input.x >= 0.9) UpdateInputs();
                    break;
            }
        }

        private void UpdateInputs()
        {
            m_qteButtonImages[m_numberOfInputs - m_requiredInputs.Count].color = Color.green;
            m_requiredInputs.Dequeue();
        }

        private void EndQTE(bool failed)
        {
            qteEventInProgress = false;
            EventBus<QTEEvent>.Raise(new QTEEvent()
            {
                Failed = failed
            });
            m_inputActions.QTE.Disable();
            m_qteCanvasGroup.alpha = 0;
        }

        private void ResetQTE()
        {
            foreach(var buttonImage in m_qteButtonImages)
            {
                buttonImage.gameObject.SetActive(false);
            }
            m_requiredInputs.Clear();
            // m_reversedInputs.Clear();
        }
    }

    [Serializable]
    public struct QTEButton
    {
        public KeyCode KeyCode;
        public Sprite Image;
    }

    public struct QTEEvent : IEvent
    {
        public bool Failed;
    }
}
