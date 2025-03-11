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

        private bool m_playing;

        private void Start()
        {
            Begin(m_difficulty);
        }

        public void Begin(float _difficulty)
        {
            m_difficulty = _difficulty;
            OnDifficultySet?.Invoke(_difficulty);
            
            OnBegin?.Invoke();

            m_playing = true;
        }

        public void Pass()
        {
            if(!m_playing) { return; }
            
            OnPassed?.Invoke();
            OnFinished?.Invoke();

            m_playing = false;
        }

        public void Fail()
        {
            if(!m_playing) { return; }
            
            OnFailed?.Invoke();
            OnFinished?.Invoke();

            m_playing = false;
        }
    }
}
