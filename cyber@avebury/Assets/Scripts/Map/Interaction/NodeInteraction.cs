using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Node), typeof(ClickableObject))]
    public class NodeInteraction : MonoBehaviour
    {
        private MinigameLoader m_loader;
        
        private Node m_node;
        private ClickableObject m_clickableObject;

        private void Awake()
        {
            m_loader = FindAnyObjectByType<MinigameLoader>();
            
            m_node = GetComponent<Node>();
            m_clickableObject = GetComponent<ClickableObject>();
            
            m_clickableObject.OnClicked.AddListener(OnClicked);
        }

        private void OnClicked()
        {
            // TODO: I mean this just isn't the way we do this, right? Maybe we use... LUA, or something?
            var minigame = m_loader.LoadMinigame(m_node.Minigame);
            if(!minigame) { return; }
            minigame.Begin(m_node.MinigameDifficulty);
        }
    }
}
