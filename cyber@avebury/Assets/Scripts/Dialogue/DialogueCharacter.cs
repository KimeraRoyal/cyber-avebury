using System;
using FMOD.Studio;
using FMODUnity;
using KR;
using UnityEngine;
using Random = UnityEngine.Random;
using STOP_MODE = FMOD.Studio.STOP_MODE;

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

        [SerializeField] private string m_name = "Name";
        
        [SerializeField] private PortraitGroup[] m_portraits;

        [SerializeField] private float m_letterDuration = 0.1f;

        [SerializeField] private Vector2 m_portraitSize = Vector2.one * 150;
        [SerializeField] private Color m_portraitColor = Color.green;
        [SerializeField] private bool m_portraitMaskable = true;
        
        [SerializeField] private EventReference m_voiceSfx;
        private EventInstance m_voiceSfxInstance;

        public string Name => m_name;

        public float LetterDuration => m_letterDuration;

        public Vector2 PortraitSize => m_portraitSize;
        public Color PortraitColor => m_portraitColor;
        public bool PortraitMaskable => m_portraitMaskable;

        public EventInstance VoiceSfx
        {
            get
            {
                if (!m_voiceSfx.IsNull && !m_voiceSfxInstance.isValid())
                {
                    m_voiceSfxInstance = RuntimeManager.CreateInstance(m_voiceSfx);
                }
                return m_voiceSfxInstance;
            }
        }

        public FrameAnimation GetPortrait(int _expression, bool _isTalking)
        {
            if (_expression >= m_portraits.Length) { _expression = 0; }
            return m_portraits[_expression].GetPortait(_isTalking);
        }

        public void PlayVoiceSfx()
        {
            if (m_voiceSfx.IsNull || !VoiceSfx.isValid()) { return; }
            
            VoiceSfx.getPlaybackState(out var playbackState);
            if (playbackState == PLAYBACK_STATE.PLAYING)
            {
                VoiceSfx.stop(STOP_MODE.IMMEDIATE);
            }
            VoiceSfx.start();
        }
    }
}