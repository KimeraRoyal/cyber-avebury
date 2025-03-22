using System;
using System.Collections.Generic;
using UnityEngine;

namespace CyberAvebury
{
    public class Dialogue : MonoBehaviour
    {
        private WordWriter m_writer;
        
        private Queue<DialogueLine> m_lines;

        private void Awake()
        {
            m_writer = GetComponentInChildren<WordWriter>();
            
            m_lines = new Queue<DialogueLine>();
        }

        private void Update()
        {
            if(m_lines.Count < 1 || m_writer.IsWriting) { return; }

            var line = m_lines.Dequeue();
            m_writer.Write(line.Line, line.Character.LetterDuration);
        }

        public void AddLine(DialogueCharacter _character, string _line)
        {
            AddLine(new DialogueLine(_character, _line));
        }

        public void AddLine(DialogueLine _line)
        {
            m_lines.Enqueue(_line);
        }
    }
}
