using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    [RequireComponent(typeof(TMP_Text))]
    public class WordWriter : MonoBehaviour
    {
        private static readonly char[] s_separators = new char[] { ' ', '\n', '.', ',', '!', '?' };
        
        private TMP_Text m_text;

        [SerializeField] private float m_preDialogueWaitDuration = 0.1f;
        [SerializeField] private float m_postDialogueWaitDuration = 0.1f;

        private bool m_isWriting;
        private bool m_break;

        private Coroutine m_currentCoroutine;

        public bool IsWriting => m_isWriting;

        public bool Break
        {
            get => m_break;
            set => m_break = value;
        }

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
            var words = _line.Split(s_separators);

            yield return new WaitForSeconds(m_preDialogueWaitDuration);

            var cursor = 0;

            for(var i = 0; i < words.Length; i++)
            {
                var word = words[i];
                if (word.Length > 0)
                {
                    var lineText = IncrementCursor(word.Length);
                    OnWordWritten?.Invoke(word);
                    OnLineUpdated?.Invoke(lineText);
                }

                if (m_break) { break; }

                for (var j = 0; j < 20 && i + 1 < words.Length && words[i + 1].Length < 1; j++)
                {
                    IncrementCursor(1);
                    i++;
                    yield return null;
                }
                IncrementCursor(1);
                
                yield return new WaitForSeconds(_letterDuration * word.Length);
                yield return new WaitUntil(() => !LoadingScreen.Instance.IsOpened);
            }

            OnLineFinished?.Invoke(_line);
            if (m_break)
            {
                m_break = false;
            }
            else
            {
                yield return new WaitForSeconds(m_postDialogueWaitDuration);
            }
            
            m_isWriting = false;
            
            yield break;

            string IncrementCursor(int _amount)
            {
                cursor += _amount;

                if (cursor >= _line.Length)
                {
                    m_text.text = _line;
                    return _line;
                }
                
                var lineText = _line[..cursor];
                var unwrittenText = _line.Substring(cursor, _line.Length - cursor);
                
                m_text.text = lineText + "<alpha=#00>" + unwrittenText;

                return lineText;
            }
        }
    }
}