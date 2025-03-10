using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace CyberAvebury.Minigame
{
    public class MinigameBase : MonoBehaviour
    {
        public UnityAction<float> OnDifficultySet;
        public UnityAction OnBegin;
        
        public UnityAction OnPassed;
        public UnityAction OnFailed;
        
        public UnityAction OnFinished;

        private float m_difficulty;

        private void Start()
        {
            Begin(Random.Range(0.0f, 1.0f));
        }

        public void Begin(float _difficulty)
        {
            m_difficulty = _difficulty;
            OnDifficultySet?.Invoke(_difficulty);
            
            OnBegin?.Invoke();
        }

        public void Pass()
        {
            OnPassed?.Invoke();
            OnFinished?.Invoke();
        }

        public void Fail()
        {
            OnFailed?.Invoke();
            OnFinished?.Invoke();
        }
    }
}
