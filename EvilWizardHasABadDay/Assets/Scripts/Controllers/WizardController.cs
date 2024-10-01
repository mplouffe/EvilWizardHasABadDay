using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace lvl_0
{
    public class WizardController : MonoBehaviour
    {
        [SerializeField]
        private Animator m_controller;

        [SerializeField]
        private float m_explosionDuration;

        private WizardState m_state;

        private Duration m_duration;

        protected void Start()
        {
            m_duration = new Duration(m_explosionDuration);
        }

        protected void Update()
        {
            switch (m_state)
            {
                case WizardState.Exploding:
                    m_duration.Update(Time.deltaTime);
                    if (m_duration.Elapsed())
                    {
                        EventBus<WizardDeadEvent>.Raise(new WizardDeadEvent());
                    }
                    break;
            }
        }

        public void Talking()
        {
            m_controller.SetTrigger("Talking");
        }

        public void Listening()
        {
            m_controller.SetTrigger("Idle");
        }

        public void Dumbfounded()
        {
            m_controller.SetTrigger("Dumbfounded");
        }

        public void Raging()
        {
            m_controller.SetTrigger("Raging");
        }

        public void Walking()
        {
            m_controller.SetTrigger("Walking");
        }

        public void Asplode()
        {
            m_controller.SetTrigger("Exploding");
            m_state = WizardState.Exploding;
        }

        public void Casting()
        {
            m_controller.SetTrigger("Casting");
        }

        private enum WizardState
        {
            Angry,
            Exploding,
            Dead
        }
    }

    public struct WizardDeadEvent : IEvent { }
}
