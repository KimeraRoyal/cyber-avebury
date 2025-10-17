using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury.LoadingScreen
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

        [SerializeField] private Animator m_tilesPrefab;
        private Animator m_tiles;

        [SerializeField] private bool m_opened;
        private bool m_showing;

        private bool m_isGlitched;

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
            
            SpawnTiles();
        }

        [Button("Show")]
        public void Show()
            => ShowScreen();

        [Button("Long Show")]
        public void LongShow()
            => ShowScreen(4.0f);

        public bool ShowScreen(float _duration = 1.0f, Action OnLoad = null)
        {
            if (m_showing) { return false; }
            StartCoroutine(Present(_duration, OnLoad));
            return true;
        }

        [Button("Glitch")]
        public void Glitch()
            => GlitchScreen(true);

        [Button("Unglitch")]
        public void Unglitch()
            => GlitchScreen(false);
        
        public void GlitchScreen(bool _glitch)
        {
            m_isGlitched = _glitch;
            m_animator.SetBool("Glitching", _glitch);
            m_tiles.SetBool("Glitching", _glitch);
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

        public void ShowTiles()
        {
            m_tiles.gameObject.SetActive(true);
        }

        public void HideTiles()
        {
            if (m_isGlitched)
            {
                // Fix for visual bug with loading screen after unglitching
                Destroy(m_tiles.gameObject);
                SpawnTiles();
                GlitchScreen(false);
            }
            
            m_tiles.gameObject.SetActive(false);
        }

        private void SpawnTiles()
        {
            m_tiles = Instantiate(m_tilesPrefab, transform);
            m_tiles.name = m_tilesPrefab.name;
            m_tiles.gameObject.SetActive(false);
        }
    }
}
