using UnityEngine;

namespace CyberAvebury.Minigames.Mainframe
{
    [RequireComponent(typeof(Minigame))]
    public class Mainframe : MonoBehaviour
    {
        private Minigame m_minigame;

        [SerializeField] private DifficultyAdjustedFloat m_timeLimitDifficulty = new (15.0f, 10.0f);
        [SerializeField] private DifficultyAdjustedInteger m_targetScoreDifficulty = new (50, 100);

        private float m_timeLimit;
        
        private int m_targetScore;
        private int m_currentScore;

        private void Awake()
        {
            m_minigame = GetComponent<Minigame>();

            m_minigame.OnDifficultySet += SetDifficulty;
        }

        private void OnDestroy()
        {
            m_minigame.OnDifficultySet -= SetDifficulty;
        }

        private void SetDifficulty(float _difficulty)
        {
            m_timeLimit = m_timeLimitDifficulty.GetValue(_difficulty);
            m_targetScore = m_targetScoreDifficulty.GetValue(_difficulty);
        }
    }
}
