using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lvl_0
{
    [Serializable]
    public struct CharacterDialogueSprites
    {
        public Speaker Character;
        public Sprite Icon;
    }

    public class HeadshotCatalogue : MonoBehaviour
    {
        [SerializeField]
        private List<CharacterDialogueSprites> m_dialogueIconEntries;

        private Dictionary<Speaker, Sprite> m_dialogueIconDictionary;

        protected void Start()
        {
            m_dialogueIconDictionary = new Dictionary<Speaker, Sprite>(m_dialogueIconEntries.Count);
            foreach (var entry in m_dialogueIconEntries)
            {
                m_dialogueIconDictionary.Add(entry.Character, entry.Icon);
            }
        }

        public Sprite GetActorIcon(Speaker actor)
        {
            if (m_dialogueIconDictionary.TryGetValue(actor, out Sprite sprite))
            {
                return sprite;
            }

            Debug.LogError($"HeadshotCatalogue: Could not find entry {actor} in the icon dictionary");
            return null;
        }
    }
}
