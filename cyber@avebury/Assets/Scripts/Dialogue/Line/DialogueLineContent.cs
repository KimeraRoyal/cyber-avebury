using System;
using FMODUnity;
using UnityEngine;

namespace CyberAvebury
{
    [Serializable]
    public class DialogueLineContent
    {
        [SerializeField] private DialogueCharacter m_character;
        [SerializeField] private int m_expression;
        
        [SerializeField] [TextArea(3, 5)] private string m_line;

        [SerializeField] private EventReference m_music;

        public DialogueCharacter Character => m_character;
        public int Expression => m_expression;
        
        public string Line => m_line;

        public EventReference Music => m_music;

        public DialogueLineContent(DialogueCharacter _character, int _expression, string _line)
        {
            m_character = _character;
            m_expression = _expression;
            
            m_line = _line;
        }
    }
}