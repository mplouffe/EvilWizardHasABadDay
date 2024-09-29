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
        private PeasantState m_state;

        protected void Start()
        {
            m_duration = new Duration();
        }

        protected void OnEnable()
        {
            ChangeState(PeasantState.Waiting);
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

        [ContextMenu("Cue Character")]
        public void CueCharacter()
        {
            ChangeState(PeasantState.Arriving);
        }

        private void ChangeState(PeasantState newState)
        {
            switch (newState)
            {
                case PeasantState.Waiting:
                    transform.position = new Vector3(m_startingXPos, transform.position.y, 0);
                    break;
                case PeasantState.Arriving:
                    m_duration.Reset(m_walkOnTime);
                    break;
                case PeasantState.Negotiating:
                    transform.position = new Vector3(m_talkingXPos, transform.position.y, 0);
                    break;
                case PeasantState.Leaving:
                    m_duration.Reset(m_walkOnTime / 2);
                    break;
            }

            m_state = newState;
        }
    }

    public enum PeasantState
    {
        Waiting,
        Arriving,
        Negotiating,
        Leaving,
    }
}
