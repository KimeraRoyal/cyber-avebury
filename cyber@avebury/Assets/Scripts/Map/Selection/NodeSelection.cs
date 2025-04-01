using CyberAvebury.Minigames;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class NodeSelection : MonoBehaviour
    {
        private MinigameLoader m_loader;
        
        private Node m_selectedNode;

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
            OnNodeSelected?.Invoke(_node);
        }

        public void LoadSelectedMinigame()
        {
            if(!m_selectedNode) { return; }
            
            var minigame = m_loader.LoadMinigame(m_selectedNode.Minigame);
            if(!minigame) { return; }
            OnNodeMinigameLoaded?.Invoke(minigame);
            
            minigame.Begin(m_selectedNode.MinigameDifficulty);
        }

        private void OnMinigameLoaded(Minigame _minigame)
        {
            _minigame.OnPassed.AddListener(OnMinigamePassed);
        }

        private void OnMinigameUnloaded(Minigame _minigame)
        {
            _minigame.OnPassed.RemoveListener(OnMinigamePassed);
        }

        private void OnMinigamePassed()
        {
            m_selectedNode.Complete();
        }
    }
}
