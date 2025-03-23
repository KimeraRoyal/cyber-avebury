using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class Dialogue : MonoBehaviour
    {
        private WordWriter m_writer;
        
        private Queue<DialogueLine> m_lines;

        private DialogueLine m_currentLine;

        public DialogueLine CurrentLine => m_currentLine;
        
        public UnityEvent<DialogueLine> OnNewLine;

        private void Awake()
        {
            m_writer = GetComponentInChildren<WordWriter>();
            
            m_lines = new Queue<DialogueLine>();
        }

        private void Update()
        {
            if(m_lines.Count < 1 || m_writer.IsWriting) { return; }

            m_currentLine = m_lines.Dequeue();
            m_writer.Write(m_currentLine.Line, m_currentLine.Character.LetterDuration);
            OnNewLine?.Invoke(m_currentLine);
        }
        
        public void AddLine(DialogueCharacter _character, int _expression, string _line)
        {
            AddLine(new DialogueLine(_character, _expression, _line));
        }

        public void AddLine(DialogueLine _line)
        {
            m_lines.Enqueue(_line);
        }
    }
}
