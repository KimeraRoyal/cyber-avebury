using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class Dialogue : MonoBehaviour
    {
        private WordWriter m_writer;
        
        private Queue<DialogueLineBase> m_upcomingDialogue;

        private DialogueLineBase m_currentDialogue;
        private int m_currentLineIndex;

        public DialogueLineBase CurrentDialogue => m_currentDialogue;
        public int CurrentLineIndex => m_currentLineIndex;
        
        public UnityEvent<DialogueLineBase> OnNewDialogue;
        public UnityEvent<DialogueLineBase, int> OnNewLine;
        
        public void AddLine(DialogueCharacter _character, int _expression, string _line)
        {
            AddLine(new DialogueLine(_character, _expression, _line));
        }

        public void AddLine(DialogueLineBase _line)
        {
            m_upcomingDialogue.Enqueue(_line);
        }

        private void Awake()
        {
            m_writer = GetComponentInChildren<WordWriter>();
            
            m_upcomingDialogue = new Queue<DialogueLineBase>();
        }

        private void Update()
        {
            if(m_writer.IsWriting) { return; }

            var nextLineIndex = m_currentLineIndex + 1;
            if (m_currentDialogue == null || nextLineIndex >= m_currentDialogue.GetLineCount())
            {
                NextDialogue();
            }
            else
            {
                ChangeLine(nextLineIndex);
            }
        }

        private void NextDialogue()
        {
            if(m_upcomingDialogue.Count < 1) { return; }
            
            m_currentDialogue = m_upcomingDialogue.Dequeue();
            OnNewDialogue?.Invoke(m_currentDialogue);
            ChangeLine(0);
        }

        private void ChangeLine(int _lineIndex)
        {
            m_currentLineIndex = _lineIndex;
            m_writer.Write(m_currentDialogue.GetLine(m_currentLineIndex), m_currentDialogue.Character.LetterDuration);
            OnNewLine?.Invoke(m_currentDialogue, m_currentLineIndex);
        }
    }
}
