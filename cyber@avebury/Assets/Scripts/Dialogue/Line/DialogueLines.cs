using System;
using UnityEngine;

namespace CyberAvebury
{
    [Serializable]
    public class DialogueLines : DialogueLineBase
    {
        [SerializeField] private DialogueLineContent[] m_lines;

        public DialogueLines(DialogueLineContent[] _lines)
        {
            m_lines = _lines;
        }

        public override DialogueLineContent GetContent(int _lineIndex)
            => m_lines[_lineIndex];

        public override int GetLineCount()
            => m_lines.Length;
    }
}