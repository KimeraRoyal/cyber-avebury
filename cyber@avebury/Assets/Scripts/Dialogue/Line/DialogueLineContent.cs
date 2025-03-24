using System;
using UnityEngine;

namespace CyberAvebury
{
    [Serializable]
    public class DialogueLineContent
    {
        [SerializeField] private int m_expression;
        
        [SerializeField] private string m_line;

        public int Expression => m_expression;
        
        public string Line => m_line;

        public DialogueLineContent(int _expression, string _line)
        {
            m_expression = _expression;
            m_line = _line;
        }
    }
}