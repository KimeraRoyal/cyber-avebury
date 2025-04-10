using UnityEngine;

namespace CyberAvebury
{
    [RequireComponent(typeof(Node), typeof(ClickableObject))]
    public class SelectableNode : MonoBehaviour
    {
        private NodeSelection m_selection;
        
        private Node m_node;
        private ClickableObject m_clickableObject;

        private void Awake()
        {
            m_selection = FindAnyObjectByType<NodeSelection>();
            
            m_node = GetComponent<Node>();
            m_clickableObject = GetComponent<ClickableObject>();
            
            m_clickableObject.OnClicked.AddListener(OnClicked);
        }

        private void OnClicked()
        {
            if(m_node.IsSubNode || m_node.CurrentState == NodeState.Locked) { return; }
            m_selection.SelectNode(m_node);
        }
    }
}
