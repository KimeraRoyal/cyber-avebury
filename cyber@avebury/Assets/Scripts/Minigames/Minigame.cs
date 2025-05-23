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

        [SerializeField] private float m_loadingScreenLength = 1.0f;
        [SerializeField] private bool m_glitchLoadingScreen;

        [SerializeField] private float m_finishedHoldDuration = 1.0f;

        private bool m_isPlaying;
        private int m_pauseCount;

        public string Description => m_description;

        public float Difficulty => m_difficulty;
        public bool IsDifficultySet => m_isDifficultySet;

        public float LoadingScreenLength => m_loadingScreenLength;
        public bool GlitchLoadingScreen => m_glitchLoadingScreen;

        public float FinishedHoldDuration
        {
            get => m_finishedHoldDuration;
            set => m_finishedHoldDuration = value;
        }

        public bool IsPlaying => m_isPlaying && !IsPaused && !LoadingScreen.Instance.IsOpened;
        public bool IsPlayingNoPause => m_isPlaying && !LoadingScreen.Instance.IsOpened;
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
            m_isPlaying = false;
            
            OnPassed?.Invoke();
            Finish();
        }

        public void Fail()
        {
            if(!m_isPlaying) { return; }
            
            OnFailed?.Invoke();
            Finish();
        }

        private void Finish()
        {
            m_isPlaying = false;
            StartCoroutine(HoldForFinish());
        }

        private IEnumerator HoldForFinish()
        {
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
