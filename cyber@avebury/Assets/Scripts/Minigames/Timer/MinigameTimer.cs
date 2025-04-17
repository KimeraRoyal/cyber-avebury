using System;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury.Minigames.Timer
{
    public class MinigameTimer : MonoBehaviour
    {
        private enum FinishedBehaviour
        {
            None,
            Pass,
            Fail
        }
        
        private Minigame m_minigame;
        
        [SerializeField] private DifficultyAdjustedFloat m_timeLimitDifficulty = new (15.0f, 10.0f);
        [SerializeField] private FinishedBehaviour m_finishedBehaviour;

        private float m_timeLimit;
        private float m_currentTime;

        public float TimeLimit => m_timeLimit;
        public float CurrentTime => m_currentTime;
        public float TimerProgress => m_currentTime / m_timeLimit;
        
        public UnityEvent<float> OnTimerUpdated;

        private void Awake()
        {
            m_minigame = GetComponentInParent<Minigame>();

            m_minigame.OnDifficultySet.AddListener(SetDifficulty);
        }

        private void OnDestroy()
        {
            m_minigame.OnDifficultySet.RemoveListener(SetDifficulty);
        }

        private void Update()
        {
            if(!m_minigame.IsPlaying) { return; }
            
            m_currentTime += Time.deltaTime;
            OnTimerUpdated?.Invoke(m_currentTime);
            
            if(m_currentTime < m_timeLimit) { return; }
            Finished();
        }

        private void Finished()
        {
            switch(m_finishedBehaviour)
            {
                case FinishedBehaviour.None:
                    break;
                case FinishedBehaviour.Pass:
                    m_minigame.Pass();
                    break;
                case FinishedBehaviour.Fail:
                    m_minigame.Fail();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetDifficulty(float _difficulty)
        {
            m_timeLimit = m_timeLimitDifficulty.GetValue(_difficulty);
        }
    }
}