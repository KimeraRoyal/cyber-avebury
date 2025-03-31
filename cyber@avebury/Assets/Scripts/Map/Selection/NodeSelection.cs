using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class NodeSelection : MonoBehaviour
    {
        private MinigameLoader m_loader;
        
        private Node m_selectedNode;

        public UnityEvent<Node> OnNodeSelected;

        private void Awake()
        {
            m_loader = FindAnyObjectByType<MinigameLoader>();
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
            minigame.Begin(m_selectedNode.MinigameDifficulty);
        }
    }
}
