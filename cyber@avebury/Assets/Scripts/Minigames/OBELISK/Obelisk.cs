using System;
using System.Collections;
using CyberAvebury.Minigames;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    [RequireComponent(typeof(Minigame))]
    public class Obelisk : MonoBehaviour
    {
        private enum MinigameState
        {
            None,
            Passed,
            Failed
        }
        
        private Dialogue m_dialogue;

        private Minigame m_minigame;

        private SubgameTransition m_transition;

        [SerializeField] private GameObject m_graphics;
        
        [SerializeField] private Minigame[] m_subgamePrefabs;
        [SerializeField] private Minigame m_finalSubgame;

        [SerializeField] private float m_minigameLoadDelay = 1.0f;
        [SerializeField] private float m_minigameLoadTime = 1.0f;

        [SerializeField] private DialogueLineObjectBase m_failureDialogue;

        [ShowInInspector] [ReadOnly]
        private int m_currentSubgameIndex;

        private bool m_loadingGame;
        
        private Minigame m_currentMinigame;
        private Minigame m_previousMinigame;
        private MinigameState m_currentState;

        public int CurrentSubgameIndex => m_currentSubgameIndex;

        public UnityEvent<int> OnSubgamesAssigned;

        public UnityEvent<int> OnBeginLoadingSubgame;

        public UnityEvent<int> OnSubgamePassed;
        public UnityEvent<int> OnSubgameFailed;

        public UnityEvent OnAllSubgamesCleared;

        private void Awake()
        {
            m_dialogue = FindAnyObjectByType<Dialogue>();

            m_minigame = GetComponent<Minigame>();

            m_transition = GetComponentInChildren<SubgameTransition>();
        }

        private void Start()
        {
            OnSubgamesAssigned?.Invoke(m_subgamePrefabs.Length);
        }

        private void Update()
        {
            if (m_loadingGame || m_currentMinigame || m_previousMinigame || LoadingScreen.Instance.IsOpened || m_dialogue.IsWriting) { return; }

            StartCoroutine(LoadSubgame(m_currentSubgameIndex));
        }

        private IEnumerator LoadSubgame(int _index)
        {
            m_loadingGame = true;

            yield return new WaitForSeconds(m_minigameLoadDelay);
            yield return new WaitUntil(() => !LoadingScreen.Instance.IsOpened && !m_dialogue.IsWriting);
            
            OnBeginLoadingSubgame?.Invoke(_index);
            
            yield return new WaitForSeconds(m_minigameLoadTime);

            m_currentMinigame = Instantiate(m_subgamePrefabs[m_currentSubgameIndex], transform);
            m_currentMinigame.gameObject.SetActive(false);
            
            m_currentMinigame.OnPassed.AddListener(OnMinigamePassed);
            m_currentMinigame.OnFailed.AddListener(OnMinigameFailed);
            m_currentMinigame.OnEnd.AddListener(OnMinigameFinished);
            
            m_transition.BeginTransition(BeginSubgame, SubgameTransition.TransitionType.FromInterlude);
            
            m_loadingGame = false;
        }

        private void BeginSubgame()
        {
            m_graphics.SetActive(false);
            
            m_currentMinigame.gameObject.SetActive(true);
            m_currentMinigame.Begin(1.0f);
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
            m_previousMinigame = m_currentMinigame;
            
            var transitionType = SubgameTransition.TransitionType.ToInterlude;
            if (m_currentState == MinigameState.Passed && m_currentSubgameIndex + 1 >= m_subgamePrefabs.Length)
            {
                OnAllSubgamesCleared?.Invoke();
                SpawnFinalMinigame();
                transitionType = SubgameTransition.TransitionType.BetweenSubgames;
            }
            m_transition.BeginTransition(FinishMinigame, transitionType);
        }

        private void FinishMinigame()
        {
            Destroy(m_previousMinigame.gameObject);
            m_previousMinigame = null;

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
            
            if (m_currentSubgameIndex >= m_subgamePrefabs.Length)
            {
                BeginSubgame();
            }
            else
            {
                m_graphics.SetActive(true);
            }
        }

        private void SpawnFinalMinigame()
        {
            m_currentMinigame = Instantiate(m_finalSubgame, transform);
            m_currentMinigame.gameObject.SetActive(false);
            
            m_currentMinigame.OnPassed.AddListener(m_minigame.Pass);
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
            m_dialogue.AddLine(m_failureDialogue.GetLine());
        }
    }
}
