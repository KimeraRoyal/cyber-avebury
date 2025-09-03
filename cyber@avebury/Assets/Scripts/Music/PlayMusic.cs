using FMODUnity;
using UnityEngine;

namespace CyberAvebury
{
    public class PlayMusic : MonoBehaviour
    {
        [SerializeField] private EventReference m_music;

        private void Start()
        {
            FindAnyObjectByType<MusicPlayer>().PlaySong(m_music);
        }
    }
}
