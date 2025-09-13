using FMODUnity;
using UnityEngine;

namespace CyberAvebury
{
    public class PlayMusic : MonoBehaviour
    {
        [SerializeField] private MusicPlayer.State m_music;

        private void Start()
        {
            MusicPlayer.Instance.ChangeMusicState(m_music);
        }
    }
}
