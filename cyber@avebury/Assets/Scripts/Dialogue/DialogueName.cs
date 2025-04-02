using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CyberAvebury
{
    [RequireComponent(typeof(TMP_Text))]
    public class DialogueName : MonoBehaviour
    {
        private const char c_minScrambleCharacter = '!';
        private const char c_maxScrambleCharacter = '~';
        
        private Dialogue m_dialogue;

        private TMP_Text m_text;

        [SerializeField] private float m_scrambleDelay = 0.1f;

        private string m_characterName;
        private bool m_scramble;
        private int m_scrambledLetters;
        private float m_timer;

        private void Awake()
        {
            m_dialogue = GetComponentInParent<Dialogue>();

            m_text = GetComponent<TMP_Text>();
            
            m_dialogue.OnNewDialogue.AddListener(OnNewDialogue);
            m_dialogue.OnEndDialogue.AddListener(OnEndDialogue);
            m_dialogue.OnNewLine.AddListener(OnNewLine);
        }

        private void Update()
        {
            if (string.IsNullOrEmpty(m_characterName) || (!m_scramble && m_scrambledLetters < 1)) { return; }

            m_timer += Time.deltaTime;
            if(m_timer < m_scrambleDelay) { return; }
            m_timer -= m_scrambleDelay;

            if(!m_scramble) { m_scrambledLetters--; }
            m_text.text = GetScrambledName();
        }

        private void OnNewDialogue(DialogueLineBase _line)
        {
            m_scramble = true;
            UpdateCharacterName(_line, 0);
        }

        private void OnEndDialogue()
        {
            m_scramble = true;
            m_scrambledLetters = m_characterName.Length;
            m_text.text = GetScrambledName();
        }

        private void OnNewLine(DialogueLineBase _line, int _lineIndex)
        {
            m_scramble = true;
            UpdateCharacterName(_line, _lineIndex);
            m_scramble = false;
        }

        private void UpdateCharacterName(DialogueLineBase _line, int _lineIndex)
        {
            m_characterName = _line.GetCharacter(_lineIndex).name.ToUpper();
            if (m_scramble) { m_scrambledLetters = m_characterName.Length; }
            
            m_text.text = m_scramble || m_scrambledLetters > 0 ? GetScrambledName() : m_characterName;
        }

        private string GetScrambledName()
        {
            var scrambledName = m_characterName[..^m_scrambledLetters];
            for (var i = m_characterName.Length - m_scrambledLetters; i < m_characterName.Length; i++)
            {
                var randomChar = (char) Random.Range(c_minScrambleCharacter, c_maxScrambleCharacter - 1);
                if (randomChar == '\\') { randomChar++; } // Prevent escape characters lol
                scrambledName += randomChar;
            }
            return scrambledName;
        }
    }
}
