using System;

namespace CyberAvebury
{
    [Serializable]
    public class DialogueLines : DialogueLineBase
    {
        private DialogueLineContent[] m_lines;

        public DialogueLines(DialogueCharacter _character, DialogueLineContent[] _lines)
            : base(_character)
        {
            m_lines = _lines;
        }

        public override int GetExpressionIndex(int _lineIndex)
            => m_lines[_lineIndex].Expression;

        public override string GetLine(int _lineIndex)
            => m_lines[_lineIndex].Line;

        public override int GetLineCount()
            => m_lines.Length;
    }
}