using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    [RequireComponent(typeof(Animator))]
    public class LoadingScreen : MonoBehaviour
    {
        private Animator m_animator;

        [SerializeField] private bool m_opened;
        private bool m_showing;

        public UnityEvent OnShow;
        public UnityEvent OnHide;

        public UnityEvent OnOpened;

        // TODO: Only one loading screen in the scene at a time. Just copy the code from the other game.

        private void Awake()
        {
            m_animator = GetComponent<Animator>();

            var loadingScreens = FindObjectsByType<LoadingScreen>(FindObjectsSortMode.None);
            if (loadingScreens.Length > 1) { Destroy(gameObject); }

            DontDestroyOnLoad(gameObject);
        }

        public bool ShowScreen(float _duration = 1.0f, Action OnLoad = null)
        {
            if (m_showing) { return false; }
            StartCoroutine(Present(_duration, OnLoad));
            return true;
        }

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
