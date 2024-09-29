using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lvl_0
{
    [Serializable]   
    public struct Interaction
    {
        public Peasant Interactor;
        public List<List<DialogueLine>> Dialogues;
        public List<KeyCode> QTEKeys;
        public float QTEDuration;
    }
}
