using UnityEngine;

namespace KR
{
    [CreateAssetMenu(fileName = "New Animation", menuName = "cyber@avebury/Frame Animation")]
    public class FrameAnimation : ScriptableObject
    {
        [SerializeField] private int m_fps = 12;
        
        [SerializeField] private Sprite[] m_frames;

        public int FPS => m_fps;

        public Sprite[] Frames => m_frames;
        
        public Sprite GetFrame(int _currentFrame)
            => m_frames[_currentFrame % m_frames.Length];
    }
}