using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    [RequireComponent(typeof(Animator))]
    public class LoadingScreen : MonoBehaviour
    {
        private static LoadingScreen s_instance;
        public static LoadingScreen Instance
        {
            get
            {
                if(!s_instance) { s_instance = FindAnyObjectByType<LoadingScreen>(); }
                return s_instance;
            }
            private set => s_instance = value;
        }

        private Animator m_animator;

        [SerializeField] private bool m_opened;
        private bool m_showing;

        public bool IsOpened => m_opened;
        public bool IsShowing => m_showing;

        public UnityEvent OnShow;
        public UnityEvent OnHide;

        public UnityEvent OnOpened;

        private void Awake()
        {
            m_animator = GetComponent<Animator>();

            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        public bool ShowScreen(float _duration = 1.0f, Action OnLoad = null)
        {
            if (m_showing) { return false; }
            StartCoroutine(Present(_duration, OnLoad));
            return true;
        }

        public void GlitchScreen(bool _glitch)
            => m_animator.SetBool("Glitching", _glitch);

        private IEnumerator Present(float _duration, Action OnLoad)
        {
            m_showing = true;
            m_animator.SetBool("Show", true);
            OnShow?.Invoke();

            yield return new WaitUntil(() => m_opened);
            OnOpened?.Invoke();
            OnLoad?.Invoke();
            yield return new WaitForSeconds(_duration);

            m_animator.SetBool("Show", false);
            m_showing = false;
            OnHide?.Invoke();
        }
    }
}
