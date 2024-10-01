using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lvl_0
{
    [Serializable]   
    public struct Interaction
    {
        public Speaker Interactor;
        public List<InteractionBeat> Beats;
        public List<DialogueBeat> DialogueBeats;
        public List<KeyCode> QTEKeys;
        public float QTEDuration;
    }

    [Serializable]
    public struct DialogueBeat
    {
        public List<DialogueLine> Dialgoue;
    }

    public enum InteractionBeat
    {
        Talking,
        Casting,
        ReactingToQTE,
        Exiting,
        Raging,
        Continuing,
        QTE
    }
}
