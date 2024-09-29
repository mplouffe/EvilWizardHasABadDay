using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace lvl_0
{
    public class GameController : SingletonBase<GameController>, IEventReceiver<DialogueCompleteEvent>
    {
        [SerializeField]
        private WizardController m_wizardController;

        [SerializeField]
        private List<PeasantEntry> m_peasantEntries;

        [SerializeField]
        private List<Interaction> m_interactions;

        [SerializeField]
        private float m_timeBetweenPeasants;

        private Dictionary<Peasant, PeasantController> m_peasantDictionary;

        private Duration m_duration;

        private GameControllerState m_state;

        private Interaction m_currentInteraction;

        private

        protected void Start()
        {
            m_duration = new Duration(m_timeBetweenPeasants);
        }

        protected void Update()
        {
            switch (m_state)
            {
                case GameControllerState.BetweenPeasants:
                    m_duration.Update(Time.deltaTime);
                    if (m_duration.Elapsed())
                    {
                        if (m_interactions.Count > 0)
                        {
                            SwitchState(GameControllerState.ActivatingInteraction);
                        }
                        else
                        {
                            SwitchState(GameControllerState.GameOver);
                        }
                    }
                    break;
            }
        }

        public void IntroComplete()
        {
            SwitchState(GameControllerState.BetweenPeasants);
        }

        private void SwitchState(GameControllerState newState)
        {
            switch (newState)
            {
                case GameControllerState.BetweenPeasants:
                    m_duration.Reset(m_timeBetweenPeasants);
                    m_state = newState;
                    break;
                case GameControllerState.ActivatingInteraction:
                    var interactionIndex = Random.Range(0, m_interactions.Count);
                    m_currentInteraction = m_interactions[interactionIndex];
                    m_interactions.RemoveAt(interactionIndex);
                    break;
            }
        }

        public void OnEvent(DialogueCompleteEvent e)
        {
            throw new NotImplementedException();
        }
    }

    public enum Peasant
    {
        Farmer
    }

    public enum GameControllerState
    {
        IntroActive,
        BetweenPeasants,
        ActivatingInteraction,
        WaitingForNextEvent,
        GameOver,
    }

    [Serializable]
    public struct PeasantEntry
    {
        public Peasant Peasant;
        public PeasantController Controller;
    }
}
