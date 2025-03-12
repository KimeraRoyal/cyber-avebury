using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace CyberAvebury.Minigames
{
    public class Minigame : MonoBehaviour
    {
        public UnityEvent<float> OnDifficultySet;
        public UnityEvent OnBegin;
        
        public UnityEvent OnPassed;
        public UnityEvent OnFailed;
        
        public UnityEvent OnFinished;

        [SerializeField] [Range(0.0f, 1.0f)] private float m_difficulty;
        private bool m_isDifficultySet;

        private bool m_isPlaying;

        public float Difficulty => m_difficulty;
        public bool IsDifficultySet => m_isDifficultySet;

        public bool IsPlaying => m_isPlaying;
        
        private void Start()
        {
            Begin(m_difficulty);
        }

        public void Begin(float _difficulty)
        {
            m_difficulty = _difficulty;
            m_isDifficultySet = true;
            OnDifficultySet?.Invoke(_difficulty);
            
            OnBegin?.Invoke();

            m_isPlaying = true;
        }

        public void Pass()
        {
            if(!m_isPlaying) { return; }
            
            OnPassed?.Invoke();
            OnFinished?.Invoke();

            m_isPlaying = false;
        }

        public void Fail()
        {
            if(!m_isPlaying) { return; }
            
            OnFailed?.Invoke();
            OnFinished?.Invoke();

            m_isPlaying = false;
        }
    }
}
