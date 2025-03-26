using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    [RequireComponent(typeof(TMP_Text))]
    public class WordWriter : MonoBehaviour
    {
        private TMP_Text m_text;

        [SerializeField] private float m_preDialogueWaitDuration = 0.1f;
        [SerializeField] private float m_postDialogueWaitDuration = 0.1f;

        private bool m_isWriting;

        private Coroutine m_currentCoroutine;

        public bool IsWriting => m_isWriting;

        public UnityEvent<string> OnLineStarted;
        public UnityEvent<string> OnLineFinished;

        public UnityEvent<string> OnWordWritten;
        public UnityEvent<string> OnLineUpdated;

        private void Awake()
        {
            m_text = GetComponent<TMP_Text>();
        }

        public void Write(string _line, float _letterDuration)
        {
            if (m_currentCoroutine != null)
            {
                StopCoroutine(m_currentCoroutine);
            }
            m_currentCoroutine = StartCoroutine(WriteLine(_line, _letterDuration));
        }

        public void Write(DialogueLineContent _content)
            => Write(_content.Line, _content.Character.LetterDuration);

        public void Write(DialogueLineBase _dialogue, int _lineIndex)
            => Write(_dialogue.GetContent(_lineIndex));
        
        private IEnumerator WriteLine(string _line, float _letterDuration)
        {
            m_isWriting = true;
            OnLineStarted?.Invoke(_line);
            
            // TODO: Maybe handle this per-letter rather than with a substring?
            var words = _line.Split(' ');

            yield return new WaitForSeconds(m_preDialogueWaitDuration);

            var cursor = 0;
            foreach (var word in words)
            {
                cursor += word.Length;
                
                var lineText = _line[..cursor];
                var unwrittenText = _line.Substring(cursor, _line.Length - cursor);
                
                m_text.text = lineText + "<alpha=#00>" + unwrittenText;
                OnWordWritten?.Invoke(word);
                OnLineUpdated?.Invoke(lineText);
                
                yield return new WaitForSeconds(_letterDuration * word.Length);

                cursor++;
            }

            OnLineFinished?.Invoke(_line);
            yield return new WaitForSeconds(m_postDialogueWaitDuration);
            
            m_isWriting = false;
        }
    }
}