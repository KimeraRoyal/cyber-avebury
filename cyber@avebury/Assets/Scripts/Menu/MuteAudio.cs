using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace CyberAvebury
{
    public class MuteAudio : MonoBehaviour
    {
        [SerializeField] private bool m_isMuted;
        
        [SerializeField] private string m_channelPath;
        
        private VCA m_channel;

        public bool IsMuted
        {
            get => m_isMuted;
            set
            {
                m_isMuted = value;
                m_channel.setVolume(value ? 0 : 1);
                OnMuteChanged?.Invoke(m_isMuted);
            }
        }
        
        public Action<bool> OnMuteChanged;

        private void Awake()
        {
            m_channel = RuntimeManager.GetVCA(m_channelPath);
        }

        private void Start()
        {
            OnMuteChanged?.Invoke(m_isMuted);
        }

        public void Mute() => IsMuted = true;
        public void Unmute() => IsMuted = false;
    }
}
