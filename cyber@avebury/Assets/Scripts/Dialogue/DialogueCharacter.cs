using System;
using KR;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CyberAvebury
{
    [CreateAssetMenu(fileName = "Character", menuName = "cyber@avebury/Character")]
    public class DialogueCharacter : ScriptableObject
    {
        [Serializable]
        private class PortraitGroup
        {
            [SerializeField] private FrameAnimation m_idle;
            [SerializeField] private FrameAnimation[] m_talking;

            public FrameAnimation GetPortait(bool _isTalking)
                => _isTalking ? GetTalk() : GetIdle();
            
            public FrameAnimation GetIdle()
                => m_idle;

            public FrameAnimation GetTalk()
                => m_talking[Random.Range(0, m_talking.Length)];
        }
        
        [SerializeField] private PortraitGroup[] m_portraits;

        [SerializeField] private float m_letterDuration = 0.1f;

        public float LetterDuration => m_letterDuration;

        public FrameAnimation GetPortrait(int _expression, bool _isTalking)
        {
            if (_expression >= m_portraits.Length) { _expression = 0; }
            return m_portraits[_expression].GetPortait(_isTalking);
        }
    }
}