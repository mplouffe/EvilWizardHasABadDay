using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace lvl_0
{
    public class OutroManager : MonoBehaviour,
        IEventReceiver<WizardDeadEvent>
    {
        [SerializeField]
        private float m_onScreenDuration;

        [SerializeField]
        private float m_fadeDuration;

        [SerializeField]
        private CanvasGroup m_introCanvas;

        [SerializeField]
        private TextMeshProUGUI m_outroText;

        [SerializeField, TextArea]
        private string m_outroWinText;

        [SerializeField, TextArea]
        private string m_outroLooseText;

        private Duration m_outroDuration;

        private bool m_fadingIn = true;

        private bool m_start = false;

        protected void Start()
        {
            m_outroDuration = new Duration(m_fadeDuration);
        }

        protected void OnEnable()
        {
            EventBus<WizardDeadEvent>.Register(this);
        }

        protected void OnDisable()
        {
            EventBus<WizardDeadEvent>.Unregister(this);
        }

        public void OnEvent(WizardDeadEvent e)
        {
            m_outroText.text = m_outroLooseText;
            m_start = true;
        }

        public void WinnerWinner()
        {
            m_outroText.text = m_outroWinText;
            m_start = true;
        }

        protected void Update()
        {
            if (!m_start)
            {
                return;
            }

            m_outroDuration.Update(Time.deltaTime);
            if (m_outroDuration.Elapsed())
            {
                if (m_fadingIn)
                {
                    m_outroDuration.Reset(m_onScreenDuration);
                    m_fadingIn = false;
                }
                else
                {
                    LevelAttendant.Instance.LoadGameState(GameState.GameOver);
                }
            }
            else
            {
                if (m_fadingIn)
                {
                    m_introCanvas.alpha = m_outroDuration.Delta();
                }
            }
        }
    }
}
