using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace CyberAvebury
{
    public enum ReturnBehaviour
    {
        None,
        StartOfLine,
        EndOfLine
    }
    
    [RequireComponent(typeof(TMP_Text))]
    public class Console : MonoBehaviour
    {
        private TMP_Text m_text;

        [SerializeField] private string[] m_startingLines;
        [SerializeField] private string m_lines;
        private Queue<string> m_queuedLines;

        [SerializeField] private float m_minLetterWriteTime = 0.1f;
        [SerializeField] private float m_maxLetterWriteTime = 0.1f;
        [SerializeField] private float m_spaceWriteTime = 0.1f;
        [SerializeField] private float m_lineEndHold = 0.1f;
        [SerializeField] private ReturnBehaviour m_returnBehaviour = ReturnBehaviour.None;
        
        [SerializeField] private string m_carat = "|";
        [SerializeField] private float m_caratBlinkInterval = 0.1f;
        private float m_caratBlinkTimer;
        private bool m_caratVisible;

        private Coroutine m_writingCoroutine;
        
        private bool m_dirty;

        public ReturnBehaviour ReturnBehaviour
        {
            get => m_returnBehaviour;
            set => m_returnBehaviour = value;
        }

        public bool Writing => m_writingCoroutine != null;

        public UnityEvent<string> OnWritingStart;
        public UnityEvent<string> OnWritingFinish;
        public UnityEvent<char> OnWriteCharacter;

        public UnityEvent OnCleared;

        public void AddLine(string _line)
            => m_queuedLines.Enqueue(_line);

        public void AddLines(string[] _lines)
        {
            foreach (var line in _lines)
            {
                m_queuedLines.Enqueue(line);
            }
        }

        public void ClearLines()
        {
            m_lines = "";
            if(m_writingCoroutine != null) { StopCoroutine(m_writingCoroutine); }
            
            OnCleared?.Invoke();
            m_dirty = true;
        }

        private void Awake()
        {
            m_text = GetComponent<TMP_Text>();

            m_queuedLines = new Queue<string>();
        }

        private void Start()
        {
            AddLines(m_startingLines);
        }

        private void Update()
        {
            BeginTyping();
            BlinkCarat();
            ShowLines();
        }

        private void BeginTyping()
        {
            if(Writing || m_queuedLines.Count < 1) { return; }
            var nextLine = m_queuedLines.Dequeue();
            m_writingCoroutine = StartCoroutine(TypeLine(nextLine));
        }

        private void BlinkCarat()
        {
            if (Writing)
            {
                m_caratBlinkTimer = 0.0f;
                return;
            }
            m_caratBlinkTimer += Time.deltaTime;
            if(m_caratBlinkTimer < m_caratBlinkInterval) { return; }
            m_caratBlinkTimer -= m_caratBlinkInterval;

            m_caratVisible = !m_caratVisible;
        }

        private void ShowLines()
        {
            m_text.text = $"{m_lines}{(Writing || m_caratVisible ? m_carat : "")}";
        }

        private IEnumerator TypeLine(string _line)
        {
            if (m_lines.Length > 0 && m_returnBehaviour == ReturnBehaviour.StartOfLine) { _line = "\n" + _line; }
            if (m_returnBehaviour == ReturnBehaviour.EndOfLine) { _line += "\n"; }

            OnWritingStart?.Invoke(_line);
            foreach (var character in _line)
            {
                m_lines += character;
                OnWriteCharacter?.Invoke(character);
                yield return new WaitForSeconds(GetCharacterWriteTime(character));
            }
            OnWritingFinish?.Invoke(_line);

            yield return new WaitForSeconds(m_lineEndHold);
            
            m_writingCoroutine = null;
        }

        private float GetCharacterWriteTime(char _character) =>
            _character switch
            {
                ' ' => m_spaceWriteTime,
                _ => Random.Range(m_minLetterWriteTime, m_maxLetterWriteTime)
            };
    }
}
