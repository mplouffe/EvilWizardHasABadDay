using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lvl_0
{
    public class IntroManager : MonoBehaviour
    {
        [SerializeField]
        private float m_onScreenDuration;

        [SerializeField]
        private float m_fadeDuration;

        [SerializeField]
        CanvasGroup m_introCanvas;

        private Duration m_introDuration;

        private bool m_fadingOut = false;

        protected void Start()
        {
            m_introDuration = new Duration(m_onScreenDuration);
        }

        protected void Update()
        {
            m_introDuration.Update(Time.deltaTime);
            if (m_introDuration.Elapsed())
            {
                if (!m_fadingOut)
                {
                    m_introDuration.Reset(m_fadeDuration);
                    m_fadingOut = true;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                if (m_fadingOut)
                {
                    m_introCanvas.alpha = 1 - m_introDuration.Delta();
                }
            }
        }
    }
}
