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

        private IEnumerator WriteLine(string _line, float _letterDuration)
        {
            m_isWriting = true;
            OnLineStarted?.Invoke(_line);
            
            var words = _line.Split(' ');
            
            var lineText = "";
            foreach (var word in words)
            {
                lineText += word;
                m_text.text = lineText;
                OnWordWritten?.Invoke(word);
                OnLineUpdated?.Invoke(lineText);
                
                yield return new WaitForSeconds(_letterDuration * word.Length);

                lineText += " ";
            }
            
            m_isWriting = false;
            OnLineFinished?.Invoke(_line);
        }
    }
}