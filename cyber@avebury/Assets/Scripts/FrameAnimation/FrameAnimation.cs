using System;
using UnityEngine;

namespace KR
{
    [CreateAssetMenu(fileName = "New Animation", menuName = "cyber@avebury/Frame Animation")]
    public class FrameAnimation : ScriptableObject
    {
        private enum LoopMode
        {
            None,
            Repeat,
            PingPong
        }
        
        [SerializeField] private int m_fps = 12;
        [SerializeField] private LoopMode m_loopMode = LoopMode.Repeat;
        
        [SerializeField] private Sprite[] m_frames;

        public int FPS => m_fps;

        public Sprite[] Frames => m_frames;

        public Sprite GetFrame(int _currentFrame)
            => m_frames[m_loopMode switch
            {
                LoopMode.None => Math.Min(_currentFrame, m_frames.Length - 1),
                LoopMode.Repeat => _currentFrame % m_frames.Length,
                LoopMode.PingPong => GetPingPongFrameIndex(_currentFrame),
                _ => throw new ArgumentOutOfRangeException()
            }];

        private int GetPingPongFrameIndex(int _currentFrame)
        {
            if (m_frames.Length <= 1) { return 0; }

            var length = m_frames.Length - 1;
            var iteration = _currentFrame / length % 2;
            var loopedIndex = _currentFrame % length;

            if (iteration == 1) { return length - loopedIndex; }
            return loopedIndex;
        }
    }
}