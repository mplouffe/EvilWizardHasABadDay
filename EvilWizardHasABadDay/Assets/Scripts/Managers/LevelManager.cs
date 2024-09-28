using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace lvl_0
{
    public class LevelManager : MonoBehaviour
    {
        private bool m_firstEventTriggered;

        private void Awake()
        {
            m_firstEventTriggered = false;
        }

        private void Update()
        {
            if (!m_firstEventTriggered)
            {
                Debug.Log("Triggering first event scene ended...");
                EventBus<EventEndedEvent>.Raise(new EventEndedEvent());
                m_firstEventTriggered = true;
            }
        }
    }
}
