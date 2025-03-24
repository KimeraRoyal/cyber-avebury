using System;
using UnityEngine;

namespace CyberAvebury
{
    [Serializable]
    public abstract class DialogueLineBase
    {
        [SerializeField] private DialogueCharacter m_character;
        
        public DialogueCharacter Character => m_character;

        protected DialogueLineBase(DialogueCharacter _character)
        {
            m_character = _character;
        }

        public abstract int GetExpressionIndex(int _lineIndex);
        public abstract string GetLine(int _lineIndex);
        public abstract int GetLineCount();
    }
}