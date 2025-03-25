using System;
using UnityEngine;

namespace CyberAvebury
{
    [Serializable]
    public abstract class DialogueLineBase
    {
        public abstract DialogueLineContent GetContent(int _lineIndex);
        
        public DialogueCharacter GetCharacter(int _lineIndex)
            => GetContent(_lineIndex).Character;
        public int GetExpressionIndex(int _lineIndex)
            => GetContent(_lineIndex).Expression;
        public string GetLine(int _lineIndex)
            => GetContent(_lineIndex).Line;
        
        public abstract int GetLineCount();
    }
}