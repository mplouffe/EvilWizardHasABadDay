using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace lvl_0
{
    public class GameController : SingletonBase<GameController>,
        IEventReceiver<DialogueCompleteEvent>, 
        IEventReceiver<ActionCompleteEvent>,
        IEventReceiver<QTEEvent>
    {
        [SerializeField]
        private WizardController m_wizardController;

        [SerializeField]
        private List<PeasantEntry> m_peasantEntries;

        [SerializeField]
        private List<Interaction> m_interactions;

        [SerializeField]
        private float m_timeBetweenPeasants;

        [SerializeField]
        private OutroManager m_outroManager;

        private Dictionary<Speaker, PeasantController> m_peasantDictionary;

        private Duration m_duration;

        private GameControllerState m_state;

        private Interaction m_currentInteraction;

        protected void Start()
        {
            m_duration = new Duration(m_timeBetweenPeasants);
            m_peasantDictionary = new Dictionary<Speaker, PeasantController>();
            foreach(var entry in m_peasantEntries)
            {
                m_peasantDictionary.Add(entry.Peasant, entry.Controller);
            }
        }

        protected void OnEnable()
        {
            EventBus<DialogueCompleteEvent>.Register(this);
            EventBus<ActionCompleteEvent>.Register(this);
            EventBus<QTEEvent>.Register(this);
        }

        protected void OnDisable()
        {
            EventBus<DialogueCompleteEvent>.Unregister(this);
            EventBus<ActionCompleteEvent>.Unregister(this);
            EventBus<QTEEvent>.Unregister(this);
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
                    var currentPeasant = m_peasantDictionary[m_currentInteraction.Interactor];
                    currentPeasant.CueCharacter();
                    m_state = GameControllerState.WaitingForNextEvent;
                    break;
                case GameControllerState.GameOver:
                    m_outroManager.WinnerWinner();
                    break;
            }
        }

        public void OnEvent(DialogueCompleteEvent e)
        {
            BeatGoesOn();
        }

        public void OnEvent(ActionCompleteEvent e)
        {
            BeatGoesOn();
        }

        public void OnEvent(QTEEvent e)
        {
            if (e.Failed)
            {
                EndOfTheWorldAsWeKnowIt();
            }
            else
            {
                BeatGoesOn();
            }
        }

        private void BeatGoesOn()
        {
            var currentBeat = m_currentInteraction.Beats[0];
            m_currentInteraction.Beats.RemoveAt(0);

            switch (currentBeat)
            {
                case InteractionBeat.Talking:
                    var currentDialogue = m_currentInteraction.DialogueBeats[0];
                    m_currentInteraction.DialogueBeats.RemoveAt(0);
                    if (currentDialogue.Dialgoue[0].Speaker == Speaker.Wizard)
                    {
                        m_wizardController.Talking();
                        m_peasantDictionary[m_currentInteraction.Interactor].Listening();
                    }
                    else
                    {
                        m_wizardController.Listening();
                        m_peasantDictionary[m_currentInteraction.Interactor].Talking();
                    }
                    DialogueController.Instance.StartDialogue(currentDialogue.Dialgoue);
                    break;
                case InteractionBeat.Casting:
                    var casting = m_currentInteraction.DialogueBeats[0];
                    m_currentInteraction.DialogueBeats.RemoveAt(0);
                    m_wizardController.Casting();
                    m_peasantDictionary[m_currentInteraction.Interactor].Scared();
                    DialogueController.Instance.StartDialogue(casting.Dialgoue);
                    break;
                case InteractionBeat.QTE:
                    QuicktimeEventManager.Instance.StartQTE(m_currentInteraction.QTEDuration, m_currentInteraction.QTEKeys);
                    break;
                case InteractionBeat.ReactingToQTE:
                    m_wizardController.Dumbfounded();
                    m_peasantDictionary[m_currentInteraction.Interactor].Happy();
                    var reacting = m_currentInteraction.DialogueBeats[0];
                    m_currentInteraction.DialogueBeats.RemoveAt(0);
                    DialogueController.Instance.StartDialogue(reacting.Dialgoue);
                    break;
                case InteractionBeat.Exiting:
                    m_peasantDictionary[m_currentInteraction.Interactor].LeavingHappy();
                    break;
                case InteractionBeat.Raging:
                    m_wizardController.Raging();
                    var raging = m_currentInteraction.DialogueBeats[0];
                    m_currentInteraction.DialogueBeats.RemoveAt(0);
                    DialogueController.Instance.StartDialogue(raging.Dialgoue);
                    break;
                case InteractionBeat.Continuing:
                    m_wizardController.Walking();
                    m_interactions.Remove(m_currentInteraction);
                    SwitchState(GameControllerState.BetweenPeasants);
                    break;

            }
        }

        private void EndOfTheWorldAsWeKnowIt()
        {
            m_wizardController.Asplode();
        }
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
        public Speaker Peasant;
        public PeasantController Controller;
    }

    public struct ActionCompleteEvent : IEvent
    { }
}
