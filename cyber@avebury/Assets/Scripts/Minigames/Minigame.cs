using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury.Minigames
{
    public class Minigame : MonoBehaviour
    {
        public UnityEvent<float> OnDifficultySet;
        public UnityEvent OnBegin;
        
        public UnityEvent OnPassed;
        public UnityEvent OnFailed;
        
        public UnityEvent OnFinished;
        public UnityEvent OnEnd;

        [SerializeField] [TextArea(3, 5)] private string m_description = "Description of the minigame and how it is played.";

        [SerializeField] [Range(0.0f, 1.0f)] private float m_difficulty;
        private bool m_isDifficultySet;

        [SerializeField] private float m_finishedHoldDuration = 1.0f;

        private bool m_isPlaying;
        private int m_pauseCount;

        public string Description => m_description;

        public float Difficulty => m_difficulty;
        public bool IsDifficultySet => m_isDifficultySet;

        public bool IsPlaying => m_isPlaying && !IsPaused;
        public bool IsPaused => m_pauseCount > 0;

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
            StartCoroutine(Finish());
        }

        public void Fail()
        {
            if(!m_isPlaying) { return; }
            
            OnFailed?.Invoke();
            StartCoroutine(Finish());
        }

        private IEnumerator Finish()
        {
            m_isPlaying = false;
            OnFinished?.Invoke();

            yield return new WaitForSeconds(m_finishedHoldDuration);
            OnEnd?.Invoke();
        }

        public void Pause()
            => m_pauseCount++;

        public void Unpause()
            => m_pauseCount = Math.Max(0, m_pauseCount - 1);
    }
}
