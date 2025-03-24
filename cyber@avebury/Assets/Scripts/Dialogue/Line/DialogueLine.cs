using System;
using UnityEngine;

namespace CyberAvebury
{
    [Serializable]
    public class DialogueLine : DialogueLineBase
    {
        [SerializeField] private DialogueLineContent m_content;

        public DialogueLine(DialogueCharacter _character, DialogueLineContent _content)
            : base(_character)
        {
            m_content = _content;
        }

        public DialogueLine(DialogueCharacter _character, int _expression, string _line)
            : this(_character, new DialogueLineContent(_expression, _line))
        {
        }

        public override int GetExpressionIndex(int _lineIndex)
            => m_content.Expression;

        public override string GetLine(int _lineIndex)
            => m_content.Line;

        public override int GetLineCount()
            => 1;
    }
}