using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace CyberAvebury.Minigames.Mainframe
{
    [RequireComponent(typeof(Minigame))]
    public class Mainframe : MonoBehaviour
    {
        private Minigame m_minigame;

        [SerializeField] private DifficultyAdjustedFloat m_timeLimitDifficulty = new (15.0f, 10.0f);
        [SerializeField] private DifficultyAdjustedInteger m_targetScoreDifficulty = new (50, 100);
        
        private int m_targetScore;
        private int m_currentScore;

        private float m_timeLimit;
        private float m_currentTime;

        public float ScoreProgress => (float)m_currentScore / m_targetScore;
        public float TimerProgress => m_currentTime / m_timeLimit;

        public UnityEvent<int> OnScoreUpdated;
        public UnityEvent<float> OnTimerUpdated;

        private void Awake()
        {
            m_minigame = GetComponent<Minigame>();

            m_minigame.OnDifficultySet.AddListener(SetDifficulty);
        }

        private void OnDestroy()
        {
            m_minigame.OnDifficultySet.RemoveListener(SetDifficulty);
        }

        private void Update()
        {
            m_currentTime += Time.deltaTime;
            OnTimerUpdated?.Invoke(m_currentTime);
            
            if(m_currentTime < m_timeLimit) { return; }
            
            m_minigame.Fail();
        }

        public void AddScore(int _score)
        {
            m_currentScore += _score;
            OnScoreUpdated?.Invoke(m_currentScore);
            
            if(m_currentScore < m_targetScore) { return; }
            
            m_minigame.Pass();
        }

        private void SetDifficulty(float _difficulty)
        {
            m_timeLimit = m_timeLimitDifficulty.GetValue(_difficulty);
            m_targetScore = m_targetScoreDifficulty.GetValue(_difficulty);
        }
    }
}
