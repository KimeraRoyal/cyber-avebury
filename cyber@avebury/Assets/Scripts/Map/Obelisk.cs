using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    [RequireComponent(typeof(Node))]
    public class Obelisk : MonoBehaviour
    {
        private Node m_node;

        public UnityEvent<NodeState> OnStateChanged;

        private void Awake()
        {
            m_node = GetComponent<Node>();
            m_node.OnStateChanged.AddListener(StateChange);
        }

        private void StateChange(NodeState _state)
            => OnStateChanged?.Invoke(_state);
    }
}
