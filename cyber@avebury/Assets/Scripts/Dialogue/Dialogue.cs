using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace CyberAvebury
{
    public class Dialogue : MonoBehaviour
    {
        private WordWriter m_writer;

        [SerializeField] private float m_openWaitTime = 1.0f;
        [SerializeField] private float m_closeWaitTime = 1.0f;
        
        private Queue<DialogueLineBase> m_upcomingDialogue;

        private DialogueLineBase m_currentDialogue;
        private int m_currentLineIndex;

        public DialogueLineBase CurrentDialogue => m_currentDialogue;
        public int CurrentLineIndex => m_currentLineIndex;
        
        public UnityEvent<DialogueLineBase> OnNewDialogue;
        public UnityEvent OnEndDialogue;
        
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
            if(m_currentDialogue != null) { return; }
            NextDialogue();
        }

        private void NextDialogue()
        {
            if(m_upcomingDialogue.Count < 1) { return; }
            
            m_currentDialogue = m_upcomingDialogue.Dequeue();
            StartCoroutine(ShowDialogue());
        }

        private void ChangeLine(int _lineIndex)
        {
            m_currentLineIndex = _lineIndex;
            m_writer.Write(m_currentDialogue.GetLine(m_currentLineIndex), m_currentDialogue.GetCharacter(m_currentLineIndex).LetterDuration);
            OnNewLine?.Invoke(m_currentDialogue, m_currentLineIndex);
        }

        private IEnumerator ShowDialogue()
        {
            OnNewDialogue?.Invoke(m_currentDialogue);
            yield return new WaitForSeconds(m_openWaitTime);

            var lineCount = m_currentDialogue.GetLineCount();
            for (var i = 0; i < lineCount; i++)
            {
                ChangeLine(i);
                yield return new WaitUntil(() => !m_writer.IsWriting);
            }
            
            OnEndDialogue?.Invoke();
            yield return new WaitForSeconds(m_closeWaitTime);
            
            m_currentDialogue = null;
        }
    }
}
