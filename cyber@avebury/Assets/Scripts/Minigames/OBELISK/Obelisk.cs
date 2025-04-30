using System;
using System.Collections;
using CyberAvebury.Minigames;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class Obelisk : MonoBehaviour
    {
        private enum MinigameState
        {
            None,
            Passed,
            Failed
        }
        
        private Dialogue m_dialogue;

        [SerializeField] private GameObject m_graphics;
        
        [SerializeField] private Minigame[] m_subgamePrefabs;

        [SerializeField] private float m_minigameLoadTime = 1.0f;

        [ShowInInspector] [ReadOnly]
        private int m_currentSubgameIndex;

        private bool m_loadingGame;
        
        private Minigame m_currentMinigame;
        private MinigameState m_currentState;

        public int CurrentSubgameIndex => m_currentSubgameIndex;

        public UnityEvent<int> OnSubgamesAssigned;

        public UnityEvent<int> OnBeginLoadingSubgame;

        public UnityEvent<int> OnSubgamePassed;
        public UnityEvent<int> OnSubgameFailed;

        private void Awake()
        {
            m_dialogue = FindAnyObjectByType<Dialogue>();
        }

        private void Start()
        {
            OnSubgamesAssigned?.Invoke(m_subgamePrefabs.Length);
        }

        private void Update()
        {
            if (m_loadingGame || m_currentMinigame || LoadingScreen.Instance.IsOpened || m_dialogue.IsWriting) { return; }

            StartCoroutine(LoadSubgame(m_currentSubgameIndex));
        }

        private IEnumerator LoadSubgame(int _index)
        {
            m_loadingGame = true;
            OnBeginLoadingSubgame?.Invoke(_index);
            
            yield return new WaitForSeconds(m_minigameLoadTime);

            m_currentMinigame = Instantiate(m_subgamePrefabs[m_currentSubgameIndex], transform);
            
            m_currentMinigame.OnPassed.AddListener(OnMinigamePassed);
            m_currentMinigame.OnFailed.AddListener(OnMinigameFailed);
            m_currentMinigame.OnEnd.AddListener(OnMinigameFinished);
            
            m_graphics.SetActive(false);
            m_currentMinigame.Begin(1.0f);
            
            // Any Loading Screen + Pausing / Unpausing Here
            
            //m_currentMinigame.OnEnd.AddListener(UnloadMinigame);
            //OnMinigameLoaded?.Invoke(m_currentMinigame);
            
            m_loadingGame = false;
        }

        private void OnMinigamePassed()
        {
            m_currentState = MinigameState.Passed;
        }

        private void OnMinigameFailed()
        {
            m_currentState = MinigameState.Failed;
        }

        private void OnMinigameFinished()
        {
            m_graphics.SetActive(true);
            
            Destroy(m_currentMinigame.gameObject);
            m_currentMinigame = null;

            switch (m_currentState)
            {
                case MinigameState.None:
                    break;
                case MinigameState.Passed:
                    PassSubgame();
                    break;
                case MinigameState.Failed:
                    FailSubgame();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            m_currentState = MinigameState.None;
        }

        [Button("Pass Subgame")]
        public void PassSubgame()
        {
            OnSubgamePassed?.Invoke(m_currentSubgameIndex);
            m_currentSubgameIndex++;
        }

        [Button("Fail Subgame")]
        public void FailSubgame()
        {
            OnSubgameFailed?.Invoke(m_currentSubgameIndex);
        }
    }
}
