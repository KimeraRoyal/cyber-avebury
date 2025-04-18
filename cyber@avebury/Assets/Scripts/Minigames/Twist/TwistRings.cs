using CyberAvebury.Minigames;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class TwistRings : MonoBehaviour
    {
        private Minigame m_minigame;

        private TwistRing[] m_rings;

        private int m_currentRingIndex;

        public TwistRing CurrentRing => m_currentRingIndex < m_rings.Length ? m_rings[m_currentRingIndex] : null;
        public int CurrentRingIndex
        {
            get => m_currentRingIndex;
            private set
            {
                if (m_currentRingIndex == value) { return; }
                m_currentRingIndex = value;
                OnCurrentRingChanged?.Invoke(m_currentRingIndex);
            }
        }

        public UnityEvent OnCorrectInput;
        public UnityEvent OnIncorrectInput;

        public UnityEvent<int> OnCurrentRingChanged;

        private void Awake()
        {
            m_minigame = GetComponentInParent<Minigame>();
            m_minigame.OnFinished.AddListener(OnMinigameFinished);

            m_rings = GetComponentsInChildren<TwistRing>();
        }

        private void Start()
        {
            foreach(var ring in m_rings)
            {
                ring.IsActive = true;
            }
        }

        private void Update()
        {
            if (!m_minigame.IsPlaying || m_minigame.IsPaused || !Input.GetMouseButtonDown(0)) { return; }

            if (CurrentRing.IsAngleValid)
            {
                CorrectInput();
                OnCorrectInput?.Invoke();
            }
            else
            {
                IncorrectInput();
                OnIncorrectInput?.Invoke();
            }
        }

        private void CorrectInput()
        {
            CurrentRing.IsActive = false;

            CurrentRingIndex++;
            if(CurrentRingIndex < m_rings.Length) { return; }

            m_minigame.Pass();
        }

        private void IncorrectInput()
        {
            CurrentRingIndex = Math.Max(0, CurrentRingIndex - 1);
            CurrentRing.IsActive = true;
        }

        private void OnMinigameFinished()
        {
            foreach (var ring in m_rings)
            {
                ring.IsActive = false;
            }
        }
    }
}
