using UnityEngine;

namespace CyberAvebury
{
    public class MenuMute : MonoBehaviour
    {
        private MuteAudio m_mute;
        
        private void Awake()
        {
            m_mute = FindAnyObjectByType<MuteAudio>();
        }

        public void Mute() => m_mute.Mute();
        public void Unmute() => m_mute.Unmute();
    }
}
