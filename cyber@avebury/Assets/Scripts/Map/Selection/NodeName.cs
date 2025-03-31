using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CyberAvebury
{
    [RequireComponent(typeof(TMP_Text))]
    public class NodeName : MonoBehaviour
    {
        private enum AnimationState
        {
            ShowIP,
            ScrambleIP,
            ChangeLength,
            UnscrambleName,
            Finished,
            Scrambling
        }

        private const char c_minScrambleCharacter = '!';
        private const char c_maxScrambleCharacter = '~';

        private NodeSelection m_selection;

        private TMP_Text m_text;

        [SerializeField] private float m_showIpDuration = 0.5f;
        [SerializeField] private float m_scrambleTime = 0.1f;

        private string m_currentValue = "192.0.32.1";
        private string m_currentName = "Node";
        private string m_currentIP = "192.0.24.1";

        private int m_scrambleStart = 0;
        private int m_scrambleLength = 0;

        private AnimationState m_currentState;
        private float m_timer;

        private void Awake()
        {
            m_selection = FindAnyObjectByType<NodeSelection>();
            m_selection.OnNodeSelected.AddListener(OnNodeSelected);

            m_text = GetComponent<TMP_Text>();

            m_currentState = AnimationState.Finished;
        }

        private void Update()
        {
            switch (m_currentState)
            {
                case AnimationState.ShowIP:
                    ShowIP();
                    break;
                case AnimationState.ScrambleIP:
                    ScrambleIP();
                    break;
                case AnimationState.ChangeLength:
                    ChangeLength();
                    break;
                case AnimationState.UnscrambleName:
                    UnscrambleName();
                    break;
                case AnimationState.Finished:
                    return;
                case AnimationState.Scrambling:
                    ScrambleCharacters();
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnNodeSelected(Node _node)
        {
            if (!_node)
            {
                UpdateText();
                m_currentState = AnimationState.Scrambling;
                return;
            }
            
            m_currentName = _node.name;
            //TODO: Assign an IP address

            m_currentValue = m_currentIP;

            m_scrambleStart = 0;
            m_scrambleLength = 0;
            
            m_currentState = AnimationState.ShowIP;
            m_timer = 0.0f;
            
            UpdateText();
        }

        private void ShowIP()
        {
            m_timer += Time.deltaTime;
            if(m_timer <= m_showIpDuration) { return; }

            m_timer = 0.0f;
            m_currentState = AnimationState.ScrambleIP;
        }

        private void ScrambleIP()
        {
            if(!ScrambleCharacters()) { return; }

            if (m_scrambleLength < m_currentValue.Length)
            {
                m_scrambleLength++;
                return;
            }
            
            m_currentState = AnimationState.ChangeLength;
        }

        private void ChangeLength()
        {
            if(!ScrambleCharacters()) { return; }

            var distance = m_currentName.Length - m_currentValue.Length;
            if (distance != 0)
            {
                var length = m_currentValue.Length + Math.Sign(distance);
                m_currentValue = "";
                for (var i = 0; i < length; i++)
                {
                    m_currentValue += "A";
                    m_scrambleLength += 1;
                }
                return;
            }

            m_currentValue = m_currentName;
            m_currentState = AnimationState.UnscrambleName;
        }

        private void UnscrambleName()
        {
            if(!ScrambleCharacters()) { return; }

            if (m_scrambleLength > 0)
            {
                m_scrambleStart++;
                m_scrambleLength--;
                return;
            }

            UpdateText();
            m_currentState = AnimationState.Finished;
        }
        
        private bool ScrambleCharacters()
        {
            m_timer += Time.deltaTime;
            if (m_timer < m_scrambleTime) { return false; }
            m_timer -= m_scrambleTime;
            
            UpdateText();
            
            return true;
        }

        private void UpdateText()
        {
            var value = "";
            if (m_scrambleLength < 1)
            {
                value = m_currentValue;
            }
            else
            {
                for (var i = 0; i < m_currentValue.Length; i++)
                {
                    var scrambled = i >= m_scrambleStart && i < m_scrambleStart + m_scrambleLength;
                    value += scrambled ? GetScrambledCharacter() : m_currentValue[i];
                }
            }
            m_text.text = value;
        }

        private char GetScrambledCharacter()
        {
            var randomChar = (char) Random.Range(c_minScrambleCharacter, c_maxScrambleCharacter - 1);
            if (randomChar == '\\') { randomChar++; } // Prevent escape characters lol
            return randomChar;
        }
    }
}
