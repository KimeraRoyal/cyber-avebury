using System;
using UnityEngine;

namespace CyberAvebury
{
    [Serializable]
    public class DialogueLineContent
    {
        [SerializeField] private DialogueCharacter m_character;
        [SerializeField] private int m_expression;
        
        [SerializeField] private string m_line;

        public DialogueCharacter Character => m_character;
        public int Expression => m_expression;
        
        public string Line => m_line;

        public DialogueLineContent(DialogueCharacter _character, int _expression, string _line)
        {
            m_character = _character;
            m_expression = _expression;
            
            m_line = _line;
        }
    }
}