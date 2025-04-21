using CyberAvebury.Minigames;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class NodeSelection : MonoBehaviour
    {
        private MinigameLoader m_loader;
        
        private Node m_selectedNode;

        private bool m_passedDirty;

        public UnityEvent<Node> OnNodeSelected;
        public UnityEvent<Minigame> OnNodeMinigameLoaded;

        private void Awake()
        {
            m_loader = FindAnyObjectByType<MinigameLoader>();
            m_loader.OnMinigameLoaded.AddListener(OnMinigameLoaded);
            m_loader.OnMinigameUnloaded.AddListener(OnMinigameUnloaded);
        }

        public void SelectNode(Node _node)
        {
            m_selectedNode = _node;
            
            _node?.Select();
            OnNodeSelected?.Invoke(_node);
        }

        public void LoadSelectedMinigame()
        {
            if(!m_selectedNode) { return; }
            m_loader.LoadMinigame(m_selectedNode.Minigame);
        }

        private void OnMinigameLoaded(Minigame _minigame)
        {
            if (!_minigame) { return; }
            OnNodeMinigameLoaded?.Invoke(_minigame);

            _minigame.Begin(m_selectedNode.MinigameDifficulty);

            _minigame.OnPassed.AddListener(OnMinigamePassed);
            _minigame.OnEnd.AddListener(OnMinigameFinished);
        }

        private void OnMinigameUnloaded(Minigame _minigame)
        {
            _minigame.OnPassed.RemoveListener(OnMinigamePassed);
            _minigame.OnEnd.RemoveListener(OnMinigameFinished);
        }

        private void OnMinigamePassed()
        {
            m_passedDirty = true;
        }
    
        private void OnMinigameFinished()
        {
            if(!m_passedDirty) { return; }
            m_passedDirty = false;

            m_selectedNode.Complete();
            SelectNode(null);
        }
    }
}
