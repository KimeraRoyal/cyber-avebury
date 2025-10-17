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

        [SerializeField] private bool m_lockWin;

        private float m_currentTime;

        private int m_targetScore;
        private int m_currentScore;

        public bool LockWin
        {
            get => m_lockWin;
            set => m_lockWin = value;
        }

        public int TargetScore => m_targetScore;
        public int CurrentScore => m_currentScore;
        public float ScoreProgress => (float)m_currentScore / m_targetScore;

        public UnityEvent<int> OnTargetScoreInitialized;
        public UnityEvent<int> OnScoreUpdated;

        public void ChangeScore(int _amount, bool _ignorePlayCheck = false)
        {
            if(!m_minigame.IsPlaying && !_ignorePlayCheck) { return; }

            m_currentScore += _amount;
            EvaluateScore();
        }

        public void SetScore(int _amount, bool _ignorePlayCheck = false)
        {
            if(!m_minigame.IsPlaying && !_ignorePlayCheck) { return; }
            
            m_currentScore = _amount;
            EvaluateScore();
        }

        private void EvaluateScore()
        {
            m_currentScore = Math.Clamp(m_currentScore, 0, m_targetScore);
            OnScoreUpdated?.Invoke(m_currentScore);
            
            if(m_lockWin || m_currentScore < m_targetScore) { return; }
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
            OnTargetScoreInitialized?.Invoke(m_targetScore);
        }
    }
}
