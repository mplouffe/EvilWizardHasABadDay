using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace lvl_0
{
    public class PeasantController : MonoBehaviour
    {
        [SerializeField]
        private float m_startingXPos;

        [SerializeField]
        private float m_talkingXPos;

        [SerializeField]
        private float m_walkOnTime;

        [SerializeField]
        private Animator m_controller;

        private Duration m_duration;
        private PeasantState m_state = PeasantState.Waiting;

        protected void Start()
        {
            m_duration = new Duration();
        }

        protected void OnEnable()
        {
            transform.position = new Vector3(m_startingXPos, transform.position.y, 0);
        }

        protected void Update()
        {
            switch (m_state)
            {
                case PeasantState.Arriving:
                    m_duration.Update(Time.deltaTime);
                    if (m_duration.Elapsed())
                    {
                        ChangeState(PeasantState.Negotiating);
                    }
                    else
                    {
                        var currentXPos = Mathf.Lerp(m_startingXPos, m_talkingXPos, m_duration.Delta());
                        transform.position = new Vector3(currentXPos, transform.position.y, 0);
                    }
                    break;
                case PeasantState.Leaving:
                    m_duration.Update(Time.deltaTime);
                    if (m_duration.Elapsed())
                    {
                        ChangeState(PeasantState.Waiting);
                    }
                    else
                    {
                        var currentXPos = Mathf.Lerp(m_talkingXPos, -m_startingXPos, m_duration.Delta());
                        transform.position = new Vector3(currentXPos, transform.position.y, 0);
                    }
                    break;
            }
        }

        public void CueCharacter()
        {
            ChangeState(PeasantState.Arriving);
        }

        public void Listening()
        {
            m_controller.SetTrigger("Idle");
        }

        public void Talking()
        {
            m_controller.SetTrigger("Talking");
        }

        public void Happy()
        {
            m_controller.SetTrigger("Happy");
        }

        public void Scared()
        {
            m_controller.SetTrigger("Recoiling");
        }

        public void LeavingHappy()
        {
            m_controller.SetTrigger("LeavingHappy");
            ChangeState(PeasantState.Leaving);
        }

        private void ChangeState(PeasantState newState)
        {
            switch (newState)
            {
                case PeasantState.Waiting:
                    transform.position = new Vector3(m_startingXPos, transform.position.y, 0);
                    EventBus<ActionCompleteEvent>.Raise(new ActionCompleteEvent());
                    break;
                case PeasantState.Arriving:
                    m_duration.Reset(m_walkOnTime);
                    break;
                case PeasantState.Negotiating:
                    transform.position = new Vector3(m_talkingXPos, transform.position.y, 0);
                    EventBus<ActionCompleteEvent>.Raise(new ActionCompleteEvent());
                    break;
                case PeasantState.Leaving:
                    m_duration.Reset(m_walkOnTime / 2);
                    break;
            }

            m_state = newState;
        }

        private enum PeasantState
        {
            Waiting,
            Arriving,
            Negotiating,
            Leaving,
        }
    }
}
