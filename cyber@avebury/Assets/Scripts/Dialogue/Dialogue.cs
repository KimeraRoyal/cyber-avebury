using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CyberAvebury
{
    public class Dialogue : MonoBehaviour
    {
        private WordWriter m_writer;

        [SerializeField] private float m_openWaitTime = 1.0f;
        [SerializeField] private float m_closeWaitTime = 1.0f;
        
        private Queue<DialogueLineBase> m_upcomingDialogue;

        private DialogueLineBase m_currentDialogue;
        private bool m_isWriting;
        private int m_currentLineIndex;
        
        private bool m_break;
        private bool m_pause;

        public DialogueLineBase CurrentDialogue => m_currentDialogue;
        public bool HasDialogue => CurrentDialogue != null || m_upcomingDialogue.Count > 0;
        public bool IsWriting => m_isWriting;

        public bool Break
        {
            get => m_break;
            set => m_break = value;
        }

        public bool Pause
        {
            get => m_pause;
            set => m_pause = value;
        }
        
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
            if(_line is not { CanUse: true }) { return; }
            m_upcomingDialogue.Enqueue(_line);
            _line.ReportUse();
        }

        private void Awake()
        {
            m_writer = GetComponentInChildren<WordWriter>();
            
            m_upcomingDialogue = new Queue<DialogueLineBase>();
        }

        private void Update()
        {
            if (m_isWriting && Input.GetKeyDown(KeyCode.B))
            {
                m_break = true;
                m_writer.Break = true;
            }
            
            if(m_currentDialogue != null) { return; }
            NextDialogue();
        }

        private void NextDialogue()
        {
            if(m_upcomingDialogue.Count < 1) { return; }
            
            m_currentDialogue = m_upcomingDialogue.Dequeue();
            m_currentLineIndex = 0;
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
            m_isWriting = true;

            yield return new WaitUntil(() => !LoadingScreen.Instance.IsOpened && !m_pause);

            OnNewDialogue?.Invoke(m_currentDialogue);
            yield return new WaitForSeconds(m_openWaitTime);

            var lineCount = m_currentDialogue.GetLineCount();
            for (var i = 0; i < lineCount && !m_break; i++)
            {
                ChangeLine(i);
                yield return new WaitUntil(() => !m_writer.IsWriting);
            }
            
            yield return new WaitUntil(() => !m_pause);

            m_isWriting = false;
            OnEndDialogue?.Invoke();
            yield return new WaitForSeconds(m_closeWaitTime);

            if (m_currentDialogue.ShouldLoadNextScene)
            {
                var loadSceneIndex = m_currentDialogue.LoadSceneIndex;
                LoadingScreen.Instance.ShowScreen(1.0f, () => SceneManager.LoadScene(loadSceneIndex));
            }
            
            m_currentDialogue = null;
            m_break = false;
        }
    }
}
