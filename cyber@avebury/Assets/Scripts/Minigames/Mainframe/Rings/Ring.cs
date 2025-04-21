using System;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury.Minigames.Mainframe.Rings
{
    public class Ring : MonoBehaviour
    {
        private Minigame m_minigame;
        
        [SerializeField] private float m_startingSize = 0.1f;
            
        [SerializeField] private DifficultyAdjustedFloat m_maxSizeDifficulty = new (2.0f, 1.0f);
        [SerializeField] private DifficultyAdjustedFloat m_totalLifetimeDifficulty = new (5.0f, 1.5f);
        
        private float m_maxSize;
        
        private float m_totalLifetime;
        private float m_currentLifetime;

        private bool m_active;

        public float CurrentSize => Mathf.Lerp(m_startingSize, m_maxSize, Progress);

        public float Progress => m_currentLifetime / m_totalLifetime;
        
        public UnityEvent<float> OnPressed;
        public UnityEvent OnFailed;
        public UnityEvent OnDeactivated;

        public UnityEvent<float> OnLifetimeUpdated;

        private void Awake()
        {
            m_minigame = GetComponentInParent<Minigame>();
    
        }

        private void Start()
        {
            if(!m_minigame.IsDifficultySet) { return; }
            SetDifficulty(m_minigame.Difficulty);
        }

        private void OnEnable()
        {
            transform.localScale = Vector3.one * m_startingSize;
            m_currentLifetime = 0.0f;
            
            m_minigame.OnDifficultySet.AddListener(SetDifficulty);
            m_minigame.OnPassed.AddListener(Press);
            m_minigame.OnFailed.AddListener(Fail);
        }

        private void OnDisable()
        {
            m_minigame.OnDifficultySet.RemoveListener(SetDifficulty);
            m_minigame.OnPassed.RemoveListener(Press);
            m_minigame.OnFailed.RemoveListener(Fail);
        }

        private void Update()
        {
            if (!m_minigame.IsPlaying || !m_active) { return; }
            
            m_currentLifetime += Time.deltaTime;
            OnLifetimeUpdated?.Invoke(m_currentLifetime);
            
            transform.localScale = Vector3.one * CurrentSize;
            
            if(m_currentLifetime < m_totalLifetime) { return; }
            Fail();
        }

        public void Press()
        {
            if(!m_active) { return; }
            
            m_active = false;
            OnPressed?.Invoke(Progress);
        }

        public void Fail()
        {
            m_active = false;
            OnFailed?.Invoke();
        }

        public void Activate()
        {
            m_active = true;
            m_currentLifetime = 0;
        }

        public void Deactivate()
        {
            OnDeactivated?.Invoke();
        }

        private void SetDifficulty(float _difficulty)
        {
            m_maxSize = m_maxSizeDifficulty.GetValue(_difficulty);
            m_totalLifetime = m_totalLifetimeDifficulty.GetValue(_difficulty);
        }
    }
}