using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(TMP_Text))]
    public class Console : MonoBehaviour
    {
        private enum ReturnBehaviour
        {
            None,
            StartOfLine,
            EndOfLine
        }
        
        private TMP_Text m_text;

        [SerializeField] private string m_lines;
        private Queue<string> m_queuedLines;

        [SerializeField] private float m_minLetterWriteTime = 0.1f;
        [SerializeField] private float m_maxLetterWriteTime = 0.1f;
        [SerializeField] private float m_spaceWriteTime = 0.1f;
        [SerializeField] private float m_lineEndHold = 0.1f;
        [SerializeField] private ReturnBehaviour m_returnBehaviour = ReturnBehaviour.None;
        private bool m_writing;
        
        [SerializeField] private string m_carat = "|";
        [SerializeField] private float m_caratBlinkInterval = 0.1f;
        private float m_caratBlinkTimer;
        private bool m_caratVisible;

        private bool m_dirty;

        public void AddLine(string _line)
            => m_queuedLines.Enqueue(_line);

        public void AddLines(string[] _lines)
        {
            foreach (var line in _lines)
            {
                m_queuedLines.Enqueue(line);
            }
        }

        private void Awake()
        {
            m_text = GetComponent<TMP_Text>();

            m_queuedLines = new Queue<string>();
        }

        private void Start()
        {
            AddLines(new[]{"Boot Sequence Initiated", "Counting Down...", "5", "4", "3", "2", "1", "Got You!"});
        }

        private void Update()
        {
            BeginTyping();
            BlinkCarat();
            ShowLines();
        }

        private void BeginTyping()
        {
            if(m_writing || m_queuedLines.Count < 1) { return; }
            var nextLine = m_queuedLines.Dequeue();
            StartCoroutine(TypeLine(nextLine));
        }

        private void BlinkCarat()
        {
            if (m_writing)
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
            m_text.text = $"{m_lines}{(m_writing || m_caratVisible ? m_carat : "")}";
        }

        private IEnumerator TypeLine(string _line)
        {
            m_writing = true;

            if (m_lines.Length > 0 && m_returnBehaviour == ReturnBehaviour.StartOfLine) { _line = "\n" + _line; }
            if (m_returnBehaviour == ReturnBehaviour.EndOfLine) { _line += "\n"; }
            
            foreach (var character in _line)
            {
                m_lines += character;
                yield return new WaitForSeconds(GetCharacterWriteTime(character));
            }

            yield return new WaitForSeconds(m_lineEndHold);
            
            m_writing = false;
        }

        private float GetCharacterWriteTime(char _character) =>
            _character switch
            {
                ' ' => m_spaceWriteTime,
                _ => Random.Range(m_minLetterWriteTime, m_maxLetterWriteTime)
            };
    }
}
