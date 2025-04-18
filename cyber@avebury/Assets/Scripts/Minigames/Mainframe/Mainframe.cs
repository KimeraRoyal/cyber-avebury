using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury.Minigames.Mainframe
{
    [RequireComponent(typeof(Minigame))]
    public class Mainframe : MonoBehaviour
    {
        private Minigame m_minigame;

        [SerializeField] private DifficultyAdjustedInteger m_targetScoreDifficulty = new (50, 100);
        
        private int m_targetScore;
        private int m_currentScore;

        public float ScoreProgress => (float)m_currentScore / m_targetScore;

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

        public void AddScore(int _score)
        {
            if(!m_minigame.IsPlaying) { return; }
            
            m_currentScore += _score;
            OnScoreUpdated?.Invoke(m_currentScore);
            
            if(m_currentScore < m_targetScore) { return; }
            
            m_minigame.Pass();
        }

        private void SetDifficulty(float _difficulty)
        {
            m_targetScore = m_targetScoreDifficulty.GetValue(_difficulty);
        }
    }
}
