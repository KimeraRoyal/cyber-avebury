using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CyberAvebury
{
    public class Nodes : MonoBehaviour
    {
        private List<Node> m_nodes;

        [SerializeField] private NodeLine m_linePrefab;

        public UnityEvent<Node> OnNodeRegistered;

        private void Awake()
        {
            m_nodes = new List<Node>();
        }

        public void RegisterNode(Node _node)
        {
            m_nodes.Add(_node);
            OnNodeRegistered?.Invoke(_node);

            if (m_nodes.Count < 2) { return; }
            for (var i = 0; i < m_nodes.Count - 1; i++)
            {
                var line = Instantiate(m_linePrefab, transform);
                line.Connect(_node, m_nodes[i]);
            }
        }
    }
}
