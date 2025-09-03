using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace CyberAvebury
{
    public class MusicPlayer : MonoBehaviour
    {
        private GUID m_currentSongGuid;
        private EventInstance m_currentSong;

        public void PlaySong(EventReference _song)
        {
            if(_song.IsNull) { return; }
            
            if (m_currentSong.hasHandle())
            {
                if(m_currentSongGuid == _song.Guid)
                {
                    m_currentSong.start();
                    return;
                }
                
                m_currentSong.stop(STOP_MODE.ALLOWFADEOUT);
                m_currentSong.release();
            }
            m_currentSong = RuntimeManager.CreateInstance(_song);
            m_currentSongGuid = _song.Guid;
            m_currentSong.start();
        }
    }
}
