using System;
using UnityEngine;

namespace CyberAvebury
{
    [Serializable]
    public class DialogueLine : DialogueLineBase
    {
        [SerializeField] private DialogueLineContent m_content;

        public DialogueLine(DialogueLineContent _content)
        {
            m_content = _content;
        }

        public DialogueLine(DialogueCharacter _character, int _expression, string _line)
            : this(new DialogueLineContent(_character, _expression, _line))
        {
            
        }

        public override DialogueLineContent GetContent(int _lineIndex)
            => m_content;

        public override int GetLineCount()
            => 1;
    }
}