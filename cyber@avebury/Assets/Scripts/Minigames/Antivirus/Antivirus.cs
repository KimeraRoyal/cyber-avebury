using System;
using CyberAvebury.Minigames;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    [RequireComponent(typeof(Minigame))]
    public class Antivirus : MonoBehaviour
    {
        private Minigame m_minigame;
        
        [SerializeField] private DifficultyAdjustedInteger m_targetScoreDifficulty = new (5, 15);

        private float m_currentTime;

        private int m_targetScore;
        private int m_currentScore;

        public int TargetScore => m_targetScore;
        public int CurrentScore => m_currentScore;
        public float ScoreProgress => (float)m_currentScore / m_targetScore;

        public UnityEvent<int> OnScoreUpdated;

        public void ChangeScore(int _amount)
        {
            m_currentScore = Math.Max(0, m_currentScore + _amount);
            OnScoreUpdated?.Invoke(m_currentScore);
            
            if(m_currentScore < m_targetScore) { return; }
            m_minigame.Pass();
        }

        private void Awake()
        {
            m_minigame = GetComponent<Minigame>();

            m_minigame.OnDifficultySet.AddListener(SetDifficulty);
        }

        private void OnDestroy()
        {
            m_minigame.OnDifficultySet.RemoveListener(SetDifficulty);
        }

        private void SetDifficulty(float _difficulty)
        {
            m_targetScore = m_targetScoreDifficulty.GetValue(_difficulty);
        }
    }
}
