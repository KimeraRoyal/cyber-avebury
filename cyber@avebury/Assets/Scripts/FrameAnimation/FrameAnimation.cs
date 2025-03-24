using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KR
{
    [CreateAssetMenu(fileName = "New Animation", menuName = "cyber@avebury/Frame Animation")]
    public class FrameAnimation : ScriptableObject
    {
        private enum LoopMode
        {
            None,
            Repeat,
            PingPong,
            Random,
            RandomNoRepeat
        }
        
        [SerializeField] private int m_fps = 12;
        [SerializeField] private LoopMode m_loopMode = LoopMode.Repeat;
        
        [SerializeField] private Sprite[] m_frames;

        public int FPS => m_fps;

        public Sprite[] Frames => m_frames;

        public Sprite GetFrame(int _currentFrame)
            => m_frames[m_frames.Length <= 1 ? 0 : m_loopMode switch
            {
                LoopMode.None => Math.Min(_currentFrame, m_frames.Length - 1),
                LoopMode.Repeat => _currentFrame % m_frames.Length,
                LoopMode.PingPong => GetPingPongFrameIndex(_currentFrame),
                LoopMode.Random => Random.Range(0, m_frames.Length),
                LoopMode.RandomNoRepeat => GetRandomFrameNoRepeat(_currentFrame),
                _ => throw new ArgumentOutOfRangeException()
            }];

        private int GetPingPongFrameIndex(int _currentFrame)
        {
            var length = m_frames.Length - 1;
            var iteration = _currentFrame / length % 2;
            var loopedIndex = _currentFrame % length;

            if (iteration == 1) { return length - loopedIndex; }
            return loopedIndex;
        }

        private int GetRandomFrameNoRepeat(int _currentFrame)
        {
            var randomIndex = Random.Range(0, m_frames.Length - 1);
            if (randomIndex >= _currentFrame) { randomIndex++; }
            return randomIndex;
        }
    }
}